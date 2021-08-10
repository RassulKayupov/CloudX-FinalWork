using System.Threading.Tasks;
using Microsoft.eShopWeb.Web.Pages.Basket;

namespace Microsoft.eShopWeb.Web.Interfaces
{
    public interface IDeliveryOrderProcessor
    {
        Task Process(BasketViewModel basketModel);
    }
}
