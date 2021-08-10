using System.Threading.Tasks;
using Microsoft.eShopWeb.Web.Pages.Basket;

namespace Microsoft.eShopWeb.Web.Interfaces
{
    public interface IBusService
    {
        Task SendOrderInQueue(BasketViewModel basket);
    }
}
