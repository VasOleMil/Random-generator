using System;

namespace CRandom
{
    public class CPassItem<TReturnItem>
	{
        //--------------------------------------------------------------------
        public long         Stt;//Execution state
        public TReturnItem  Res;//Working result
        public long         Rew;//Warning code
        public long         Ree;//Error code

        //--------------------------------------------------------------------
        public CPassItem()
        {
            Stt = 0L;
            Rew = 0L;
            Ree = 0L;
        }
    }
}