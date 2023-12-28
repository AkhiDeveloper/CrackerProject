namespace BookManager.API.Extension
{
    public static class StringCustomFormatter
    {
        public static bool TrySeperateStartNumber
            (this string inputString, out string startNumber, out string remainingString)
        {
            startNumber = String.Empty; 
            remainingString = String.Empty;
            inputString = inputString.Trim();

            //checking each character
            int breakingIndex = 0;
            for(int i = 0; i < inputString.Count(); i++)
            {
                char letter = inputString[i];

                //break loop if digit not found
                if (!Char.IsDigit(letter))
                {
                    breakingIndex = i;
                    break;
                }
            }

            //returning if not start with number
            if(breakingIndex == 0)
            {
                return false;
            }

            //assigning startnumber
            startNumber = inputString.Substring(0, breakingIndex);
            remainingString = inputString.Substring
                (breakingIndex, inputString.Count() - breakingIndex);

            return true;
        }

        public static string TrimSymbolStart(this string inputString)
        {
            string result = string.Empty;
            inputString = inputString.Trim();
            int breakIndex = 0;
            for(int i = 0; i < inputString.Length; i++)
            {
                if (Char.IsLetterOrDigit(inputString[i]))
                {
                    breakIndex = i;
                    break;
                }
            }

            var outputString = inputString.Substring(breakIndex, inputString.Count() - breakIndex);
            return outputString;
        }
    }
}
