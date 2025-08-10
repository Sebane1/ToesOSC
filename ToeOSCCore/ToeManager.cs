using LucHeart.CoreOSC;
using System.Net.Sockets;

namespace ToeOSCCore {
    public class ToeManager {
        private UdpClient _udpSender;
        private Dictionary<string, bool> persistedValues = new Dictionary<string, bool>();
        public ToeManager() {
            _udpSender = new UdpClient();
            _udpSender.Connect("127.0.0.1", 9000);
        }

        public void SetToeValue(int toeNumber, FootSide footSide, bool value) {
            string valueKey = $"/avatar/parameters/Toe{footSide.ToString()}" + toeNumber;
            OscMessage oscMessage = new OscMessage(valueKey, value);
            _udpSender.SendAsync(oscMessage.GetBytes());
            persistedValues[valueKey] = value;
        }

        public bool GetToeValue(int toeNumber, FootSide footSide) {
            string valueKey = $"/avatar/parameters/Toe{footSide.ToString()}" + toeNumber;
            return persistedValues.ContainsKey(valueKey) ? persistedValues[valueKey] : false;
        }
        public enum FootSide {
            Left = 0,
            Right = 1
        }
    }
}
