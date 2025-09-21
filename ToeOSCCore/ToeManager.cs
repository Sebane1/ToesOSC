using Everything_To_IMU_SlimeVR.Tracking;
using ImuToXInput;
using LucHeart.CoreOSC;
using System.Net.Sockets;
using System.Numerics;

namespace ToeOSCCore
{
    public class ToeManager : IDisposable
    {
        private UdpClient _udpSender;
        private Dictionary<string, bool> persistedValues = new Dictionary<string, bool>();
        private bool _disposed;
        private SlimeVRClient _slimeVrClient;

        public ToeManager()
        {
            _udpSender = new UdpClient();
            _udpSender.Connect("127.0.0.1", 9000);
            _slimeVrClient = new SlimeVRClient();
            _slimeVrClient.UsesSkeletalRotation = false;
            _slimeVrClient.NewDataReceived += delegate { ToeLoop(); };
            _slimeVrClient.Start();
        }


        public void ToeLoop()
        {
            try
            {
                Console.SetCursorPosition(0, 0);

                // LEFT FOOT + TOES
                if (_slimeVrClient.Trackers.TryGetValue("LEFT_FOOT", out var leftFoot) &&
                    _slimeVrClient.Trackers.TryGetValue("Toes Left", out var leftToes))
                {
                    ProcessToeSide(leftFoot, leftToes, FootSide.Left);
                    _lastLeftFoot = leftFoot;
                    _lastLeftToes = leftToes;
                }

                // RIGHT FOOT + TOES
                if (_slimeVrClient.Trackers.TryGetValue("RIGHT_FOOT", out var rightFoot) &&
                    _slimeVrClient.Trackers.TryGetValue("Toes Right", out var rightToes))
                {
                    ProcessToeSide(rightFoot, rightToes, FootSide.Right);
                    _lastRightFoot = rightFoot;
                    _lastRightToes = rightToes;
                }
                if (!calibrated)
                {
                    CalibrateToes(leftFoot, _lastLeftToes, rightFoot, _lastRightToes);
                    calibrated = true;
                }
                // Optional: debug for NONE tracker
                if (_slimeVrClient.Trackers.TryGetValue("NONE", out var none))
                {
                    Console.WriteLine(none.Euler);
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"ToeLoop error: {ex.Message}");
            }
        }

        // Store these per foot after calibration
        private Vector3 _leftToeRestAxis;
        private Vector3 _rightToeRestAxis;
        private bool calibrated;
        private TrackerState _lastLeftFoot;
        private TrackerState _lastLeftToes;
        private TrackerState _lastRightFoot;
        private TrackerState _lastRightToes;
        private Quaternion _leftToeRestRelative;
        private Quaternion _rightToeRestRelative;

        public void CalibrateToes(TrackerState leftFoot, TrackerState leftToes,
                                  TrackerState rightFoot, TrackerState rightToes)
        {
            // Store foot-relative toe orientation in world space (uncalibrated)
            _leftToeRestRelative = Quaternion.Normalize(Quaternion.Inverse(leftFoot.Rotation) * leftToes.Rotation);
            _rightToeRestRelative = Quaternion.Normalize(Quaternion.Inverse(rightFoot.Rotation) * rightToes.Rotation);

            Console.WriteLine($"Calibration complete.\nLeft: {_leftToeRestRelative}\nRight: {_rightToeRestRelative}");
        }

        private void ProcessToeSide(TrackerState foot, TrackerState toes, FootSide side)
        {
            // 1. Current relative rotation (raw world space)
            Quaternion currentRelative = Quaternion.Normalize(Quaternion.Inverse(foot.Rotation) * toes.Rotation);

            // 2. Rest relative rotation captured during calibration
            Quaternion restRelative = (side == FootSide.Left) ? _leftToeRestRelative : _rightToeRestRelative;

            // 3. Difference between current and rest
            Quaternion delta = Quaternion.Normalize(Quaternion.Inverse(restRelative) * currentRelative);

            // 4. Interpret toe bending as rotation around local X (hinge axis)
            Vector3 hingeAxis = Vector3.UnitX;
            Vector3 rotatedAxis = Vector3.Transform(hingeAxis, delta);

            // 5. Angle between rest and current hinge axis
            float angle = MathF.Acos(Math.Clamp(Vector3.Dot(Vector3.Normalize(hingeAxis),
                                                            Vector3.Normalize(rotatedAxis)), -1f, 1f));
            float angleDeg = angle * (180f / MathF.PI);

            // 6. Boolean threshold
            bool toesBending = angleDeg > 20f;

            for (int i = 0; i < 5; i++)
                SetToeValue(i, side, toesBending);

            Console.WriteLine($"Foot ({side}) toe delta: {angleDeg:F1}°, bending: {toesBending}");
            Console.WriteLine($"Foot ({side}) toe euler: {delta.QuaternionToEuler():F1}°, bending: {toesBending}");
        }


        // Helper: normalize angle to [-180,180]
        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle > 180f) angle -= 360f;
            if (angle < -180f) angle += 360f;
            return angle;
        }

        public void SetToeValue(int toeNumber, FootSide footSide, bool value)
        {
            string valueKey = $"/avatar/parameters/Toe{footSide.ToString()}" + toeNumber;
            if (!persistedValues.ContainsKey(valueKey) || persistedValues[valueKey] != value)
            {
                OscMessage oscMessage = new OscMessage(valueKey, value);
                _udpSender.SendAsync(oscMessage.GetBytes());
                persistedValues[valueKey] = value;
            }
        }

        public bool GetToeValue(int toeNumber, FootSide footSide)
        {
            string valueKey = $"/avatar/parameters/Toe{footSide.ToString()}" + toeNumber;
            return persistedValues.ContainsKey(valueKey) ? persistedValues[valueKey] : false;
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public enum FootSide
        {
            Left = 0,
            Right = 1
        }
    }
}
