namespace Domain.Util
{
    public class VerifyChar
    {
        public virtual bool ContainsInvalidCharacter(string pString, string character)
        {
            var chars = character.ToCharArray();
            foreach (char x in chars)
            {
                if (pString.Contains(x.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}