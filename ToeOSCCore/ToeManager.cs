using Everything_To_IMU_SlimeVR.Tracking;
using ImuToXInput;
using LucHeart.CoreOSC;
using System.Collections.Concurrent;
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
                if (_slimeVrClient.Trackers.TryGetValue("LEFT_FOOT", out var leftFoot))
                {
                    if (_slimeVrClient.Trackers.TryGetValue("Toes Left", out var leftToes))
                    {
                        CalibrateToes(leftFoot, leftToes);
                        for (int i = 0; i < 5; i++)
                        {
                            ProcessToeSide(leftFoot, leftToes, FootSide.Left, i);
                        }
                    }
                    if (_slimeVrClient.Trackers.TryGetValue("Big Toe Left", out var bigToeLeft))
                    {
                        CalibrateToes(bigToeLeft, bigToeLeft);
                        ProcessToeSide(leftFoot, bigToeLeft, FootSide.Left, 1);
                    }
                    if (_slimeVrClient.Trackers.TryGetValue("Other Toes Left", out var otherToesLeft))
                    {
                        CalibrateToes(otherToesLeft, otherToesLeft);
                        for (int i = 1; i < 5; i++)
                        {
                            ProcessToeSide(leftFoot, otherToesLeft, FootSide.Left, i);
                        }
                    }
                }

                // RIGHT FOOT + TOES
                if (_slimeVrClient.Trackers.TryGetValue("RIGHT_FOOT", out var rightFoot))
                {
                    if (_slimeVrClient.Trackers.TryGetValue("Toes Right", out var rightToes))
                    {
                        CalibrateToes(rightFoot, rightToes);
                        for (int i = 0; i < 5; i++)
                        {
                            ProcessToeSide(rightFoot, rightToes, FootSide.Right, i);
                        }
                    }
                    if (_slimeVrClient.Trackers.TryGetValue("Big Toe Right", out var bigToesRight))
                    {
                        CalibrateToes(bigToesRight, bigToesRight);
                        ProcessToeSide(rightFoot, bigToesRight, FootSide.Right, 1);
                    }
                    if (_slimeVrClient.Trackers.TryGetValue("Other Toes Right", out var otherToesRight))
                    {
                        CalibrateToes(otherToesRight, otherToesRight);
                        for (int i = 1; i < 5; i++)
                        {
                            ProcessToeSide(rightFoot, otherToesRight, FootSide.Right, i);
                        }
                    }
                    // Optional: debug for NONE tracker
                    if (_slimeVrClient.Trackers.TryGetValue("NONE", out var none))
                    {
                        Console.WriteLine(none.Euler);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"ToeLoop error: {ex.Message}");
            }
        }
        ConcurrentDictionary<string, Quaternion> _toeCalibrationDictionary = new ConcurrentDictionary<string, Quaternion>();

        public void CalibrateToes(TrackerState foot, TrackerState toeTracker)
        {
            if (!_toeCalibrationDictionary.ContainsKey(toeTracker.BodyPart))
            {
                // Store foot-relative toe orientation in world space (uncalibrated)
                _toeCalibrationDictionary[toeTracker.BodyPart] = Quaternion.Normalize(Quaternion.Inverse(foot.RotationCalibrated) * toeTracker.RotationCalibrated);
                Console.WriteLine($"Calibration complete.\n{toeTracker.BodyPart}:{_toeCalibrationDictionary[toeTracker.BodyPart]}");
            }
        }

        private void ProcessToeSide(TrackerState foot, TrackerState toe, FootSide side, int toeNumber)
        {
            Quaternion currentRelative = Quaternion.Normalize(Quaternion.Inverse(foot.RotationCalibrated) * toe.RotationCalibrated);
            bool toesBending = currentRelative.QuaternionToEuler().X > 30f;

            SetToeValue(toeNumber, side, toesBending);

            Console.WriteLine($"Foot ({toe.BodyPart}) toe local euler: {currentRelative.QuaternionToEuler():F1}°, bending: {toesBending}");
            Console.WriteLine($"Foot ({toe.BodyPart}) toe world euler: {toe.RotationCalibrated.QuaternionToEuler():F1}°, bending: {toesBending}");
            Console.WriteLine($"Foot ({toe.BodyPart}) foot world euler: {foot.RotationCalibrated.QuaternionToEuler()}");
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
