using System;
using System.Collections.Generic;


namespace TDEngine {

    static class Extentions
    {

        // Int Extentions:
        public static int randomInt(this int self, int min, int max)
        {
            Random random = new Random();
            int result = random.Next(min, max);
            return result;
        }

        // String Extentions:

    }
}
