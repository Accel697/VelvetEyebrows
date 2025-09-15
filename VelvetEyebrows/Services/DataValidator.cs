using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelvetEyebrows.Model;

namespace VelvetEyebrows.Services
{
    internal class DataValidator
    {
        public (bool isValid, List<string> errors) ServiceValidator(Service service)
        {
            List<string> errors = new List<string>();

            if (service.Title.Length < 2 || service.Title.Length > 100)
                errors.Add("Название должно содержать от 2 до 100 символов");

            if (service.Cost <= 0)
                errors.Add("Укажите стоимость");

            if (service.DurationInSeconds <= 0)
                errors.Add("Укажите длительность");

            if (service.Discount < 0 || service.Discount >= 100)
                errors.Add("Скидка должна быть от 0 до 100%");

            return (errors.Count == 0, errors);
        }

        public (bool isValid, List<string> errors) RequestValidator(ClientService request)
        {
            List<string> errors = new List<string>();

            if (request.ClientID == 0)
                errors.Add("Выберите клиента");

            if (request.ServiceID == 0)
                errors.Add("Выберите услугу");

            if (request.StartTime == null)
                errors.Add("Укажите дату и время");

            return (errors.Count == 0, errors);
        }
    }
}
