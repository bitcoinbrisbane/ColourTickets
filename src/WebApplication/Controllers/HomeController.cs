using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var db = new LiteDatabase(@"c:\temp\MyData.db"))
            {
                // Get customer collection
                LiteCollection<Models.EventModel> events = db.GetCollection<Models.EventModel>("events");

                return View(events.FindAll());
            }
        }

        public async Task<ActionResult> Create()
        {
            using (Colu.IAddressClient client = new Colu.Client("http://192.168.0.12:8080", "bitcoinbrisbane", "Test1234"))
            {
                var address = await client.GetAddressAsync("1");

                Models.CreateEventViewModel model = new Models.CreateEventViewModel()
                {
                    Id = address.Address
                };

                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Models.CreateEventViewModel viewModel)
        {
            using (var db = new LiteDatabase(@"c:\temp\MyData.db"))
            {
                Models.EventModel model = new Models.EventModel()
                {
                    Description = viewModel.Description,
                    EndDate = viewModel.EndDate,
                    StartDate = viewModel.StartDate,
                    Name = viewModel.Name,
                    Id = viewModel.Id
                };

                using (Colu.Client client = new Colu.Client("http://192.168.0.12:8080", "bitcoinbrisbane", "Test1234"))
                {
                    var request = new Colu.Models.IssueAsset.Request()
                    {
                        Param = new Colu.Models.IssueAsset.AssetParams()
                        {
                            Amount = 250,
                            IssueAddress = viewModel.Id,
                            Divisibility = 0,
                            Reissueable = true,
                            MetaData = new Colu.Models.IssueAsset.MetaData()
                            {
                                AssetName = viewModel.Name
                            }
                        }
                    };

                    var result = await client.IssueAsync(request);
                }

                LiteCollection<Models.EventModel> events = db.GetCollection<Models.EventModel>("events");
                events.Insert(model);

                return RedirectToAction("Index");
            }
        }

        public ActionResult Buy(String id)
        {
            Models.BuyTicketViewModel viewModel = new Models.BuyTicketViewModel()
            {
                Id = id
            };

            return View(viewModel);
        }
    }
}