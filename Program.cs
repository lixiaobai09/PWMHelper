using System;
using igfxDHLib;

namespace PWMHelper
{    
    class Program
    {
        private const int MinFrequency = 200;
        private const int MaxFrequency = 25000;
        private static DataHandler _dh = new DataHandler();
        
        [STAThread]
        public static void Main(string[] args)
        {
            int targetPwmFrequency = 2000;
            uint error = 0;
            byte[] baseData = new byte[8];

            _dh.GetDataFromDriver(ESCAPEDATATYPE_ENUM.GET_SET_PWM_FREQUENCY, 4, ref error, ref baseData[0]);
            if (baseData == null)
                return;
            
            int currentPwmFrequency = BitConverter.ToInt32(baseData, 4);
            
            if (currentPwmFrequency == targetPwmFrequency)
                return;

            if (targetPwmFrequency < MinFrequency) targetPwmFrequency = MinFrequency;
            if (targetPwmFrequency > MaxFrequency) targetPwmFrequency = MaxFrequency;

            Array.Copy(BitConverter.GetBytes(targetPwmFrequency), 0, baseData, 4, 4);
            _dh.SendDataToDriver(ESCAPEDATATYPE_ENUM.GET_SET_PWM_FREQUENCY, 4, ref error, ref baseData[0]);
        }
    }
}
