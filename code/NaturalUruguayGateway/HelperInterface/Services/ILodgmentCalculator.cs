using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.HelperInterface.Services
{
    public interface ILodgmentCalculator
    {
        Task<double> CalculateTotalStayAsync(LodgmentOptionsModel lodgmentOptionsModel, Lodgment lodgment);
    }
}