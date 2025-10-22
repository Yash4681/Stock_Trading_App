using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.Helpers
{
    public class DateAndTimeOfOrderValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value == null) return false;

            DateTime minimumDate = new DateTime(2000, 01, 01);
            
            if(DateTime.Parse(value.ToString()) < minimumDate)
            {
                return false;
            }

            return true;
        }
    }
}
