using Microsoft.AspNetCore.Mvc;
using MvcCosmosDbCedex.Services;

namespace MvcCosmosDbCedex.Controllers
{
    public class CochesController : Controller
    {
        private ServiceCosmosDb service;

        public CochesController(ServiceCosmosDb service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string dato)
        {
            await this.service.CreateDatabaseAsync();
            ViewData["MENSAJE"] = "Cosmos generado correctamente";
            return View();
        }

        public async Task<IActionResult> Vehiculos()
        {
            List<Vehiculo> cars = await this.service.GetVehiculosAsync();
            return View(cars);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create
            (Vehiculo car, string dalegas)
        {
            if (dalegas != null)
            {
                car.Motor = new Models.Motor();
                car.Motor.Tipo = "Gasolina";
                car.Motor.Cilindrada = 9000;
                car.Motor.Caballos = 250;
            }
            await this.service.CreateVehiculoAsync(car);
            return RedirectToAction("Vehiculos");
        }

        public async Task<IActionResult> Details(int id)
        {
            Vehiculo car = await this.service.FindVehiculoAsync(id);
            return View(car);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Vehiculo car = await this.service.FindVehiculoAsync(id);
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Vehiculo car)
        {
            await this.service.UpdateVehiculo(car);
            return RedirectToAction("Vehiculos");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.service.DeleteVehiculo(id);
            return RedirectToAction("Vehiculos");
        }
    }
}
