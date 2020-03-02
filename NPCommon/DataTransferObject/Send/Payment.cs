using System;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Payment
    {
        private Car mCar = new Car();
        private SeasonCar mseasonCar = new SeasonCar();

        public Car car
        {
            set { mCar = value; }
            get { return mCar; }
        }
        
        public SeasonCar seasonCar
        {
            set { mseasonCar = value; }
            get { return mseasonCar; }
        }
    }
}