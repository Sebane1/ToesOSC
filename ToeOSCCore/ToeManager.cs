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

public void CalibrateToes(TrackerState leftFoot, TrackerState leftToes,
                                  TrackerState rightFoot, TrackerState rightToes)
        {
            // Capture foot-relative toe hinge axis at neutral pose
            _leftToeRestAxis = Vector3.Transform(Vector3.UnitX,
                                Quaternion.Normalize(Quaternion.Inverse(leftFoot.RotationCalibrated) * leftToes.RotationCalibrated));

            _rightToeRestAxis = Vector3.Transform(Vector3.UnitX,
                                Quaternion.Normalize(Quaternion.Inverse(rightFoot.RotationCalibrated) * rightToes.RotationCalibrated));

            Console.WriteLine($"Calibration complete. Left rest axis: {_leftToeRestAxis}, Right rest axis: {_rightToeRestAxis}");
        }

        private void ProcessToeSide(TrackerState foot, TrackerState toes, FootSide side)
        {
            // 1. Toe rotation relative to foot
            Quaternion toeLocalRot = Quaternion.Normalize(Quaternion.Inverse(foot.RotationCalibrated) * toes.RotationCalibrated);

            // 2. Toe hinge axis in foot-local space (X-axis)
            Vector3 toeHingeWorld = Vector3.Transform(Vector3.UnitX, toeLocalRot);

            // 3. Compute foot plane using foot's up vector (Y axis)
            Vector3 footUp = Vector3.Transform(Vector3.UnitY, foot.RotationCalibrated);

            // 4. Project hinge axis onto foot plane
            Vector3 projectedHinge = toeHingeWorld - Vector3.Dot(toeHingeWorld, footUp) * footUp;

            if (projectedHinge.LengthSquared() < 1e-6f)
                projectedHinge = Vector3.UnitX; // fallback if almost zero

            projectedHinge = Vector3.Normalize(projectedHinge);

            // 5. Pre-calibrated rest axis, also projected onto foot plane
            Vector3 toeHingeRest = (side == FootSide.Left) ? _leftToeRestAxis : _rightToeRestAxis;

            // Project rest axis onto same plane
            Vector3 projectedRest = toeHingeRest - Vector3.Dot(toeHingeRest, footUp) * footUp;
            projectedRest = Vector3.Normalize(projectedRest);

            // 6. Angle between projected vectors
            float deltaRad = MathF.Acos(Math.Clamp(Vector3.Dot(projectedHinge, projectedRest), -1f, 1f));
            float deltaDeg = deltaRad * (180f / MathF.PI);

            // 7. Boolean threshold for bending
            bool toesBending = deltaDeg > 20f;

            // 8. Send OSC boolean for all toes
            for (int i = 0; i < 5; i++)
                SetToeValue(i, side, toesBending);

            // Debug
            Console.WriteLine($"Foot ({side}) toe projected delta: {deltaDeg:F1}°, bending: {toesBending}");
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
