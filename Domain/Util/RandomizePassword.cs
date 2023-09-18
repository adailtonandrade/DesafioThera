using System;
using System.Linq;

namespace Domain.Util
{
    public static class RandomizePassword
    {
        private static string[] allowedDigits = {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };
        private static string[] allowedUpperChars = {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };
        private static string[] allowedLowerChars = {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };
        private static string[] allowedNonNumber = {
            "!", "@", "#", "$", "%", "&", "?"
        };
        private static string allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z," +
        "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
        "1,2,3,4,5,6,7,8,9,0," +
        "!,@,#,$,%,&,?";
        private static char[] sep = { ',' };
        private static string[] arr = allowedChars.Split(sep);
        private static Random rand = new Random();
        public static string GenerateRandom(int length)
        {
            var passwordString = "";
            passwordString += Randomize(length / 2);
            if (!VerifyDigits(passwordString)) { passwordString += allowedDigits[rand.Next(0, allowedDigits.Count())]; }
            if (!VerifyUpperChars(passwordString)) { passwordString += allowedUpperChars[rand.Next(0, allowedUpperChars.Count())]; }
            if (!VerifyLowerChars(passwordString)) { passwordString += allowedLowerChars[rand.Next(0, allowedLowerChars.Count())]; }
            if (!VerifyNonNumber(passwordString)) { passwordString += allowedNonNumber[rand.Next(0, allowedNonNumber.Count())]; }

            var newLength = length - passwordString.Count();
            passwordString += Randomize(newLength);
            return passwordString;
        }

        private static string Randomize(int length)
        {
            string passwordString = "";
            string temp = "";
            for (int i = 0; i < length; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }

        private static bool VerifyDigits(string password)
        {
            var result = allowedDigits.Where(e => password.Contains(e)).ToList();
            return result.Count > 0;
        }
        private static bool VerifyUpperChars(string password)
        {
            var result = allowedUpperChars.Where(e => password.Contains(e)).ToList();
            return result.Count > 0;
        }
        private static bool VerifyLowerChars(string password)
        {
            var result = allowedLowerChars.Where(e => password.Contains(e)).ToList();
            return result.Count > 0;
        }
        private static bool VerifyNonNumber(string password)
        {
            var result = allowedNonNumber.Where(e => password.Contains(e)).ToList();
            return result.Count > 0;
        }
    }
}
