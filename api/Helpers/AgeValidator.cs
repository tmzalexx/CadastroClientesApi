namespace CadastroClientesApi.api.Helpers;

    public static class AgeValidator
    {
        public static int CalculateAge(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - dateOfBirth.Value.Year;
                if (dateOfBirth > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
            return 0;
        }
    }

