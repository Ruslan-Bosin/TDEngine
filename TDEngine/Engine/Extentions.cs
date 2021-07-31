using System;
using System.Collections.Generic;

namespace TDEngine {

    static class Extentions {

        // Int Extentions:
        public static int randomInt(this int self, int min, int max) {
            Random random = new Random();
            int result = random.Next(min, max);
            return result;
        }

        public static int percentageRatio(this int self, int percentages) {
            double calculate = (self / 100) * percentages;
            int result = (int)Math.Round(calculate, 0);
            return result;
        }

        public static void print(this String line) {
            System.Windows.Forms.MessageBox.Show(line);
        }

    }
}
