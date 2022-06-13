using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.DAL;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Manage.Controllers
{
    [Area("Manage"), Authorize]
    public class CarController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;

        public CarController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            ViewBag.Brands = _context.Brands.ToList();
            return View(_context.Cars.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Create(Car car)
        {
            if (!ModelState.IsValid) return View(car);
            if (car.ImageFile != null)
            {
                if (car.ImageFile.ContentType != "image/jpeg" && car.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View(car);
                }
                if (car.ImageFile.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View(car);
                }

                string filename = car.ImageFile.FileName;
                if (filename.Length > 64)
                {
                    filename.Substring(filename.Length - 64, 64);
                }
                string newFileName = Guid.NewGuid().ToString() + filename;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    car.ImageFile.CopyTo(stream);
                }
                car.Image = newFileName;
                _context.Cars.Add(car);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Car car = _context.Cars.Include(x => x.Brand).FirstOrDefault(x => x.Id == id);
            if (car == null) return NotFound();
            ViewBag.Brands = _context.Brands.ToList();

            return View(car);
        }

        [HttpPost]
        public IActionResult Edit(Car car)
        {
            var existCar = _context.Cars.Include(x => x.Brand).FirstOrDefault(x => x.Id == car.Id);
            if (existCar == null) return NotFound();
            string newFileName = null;

            if (car.ImageFile != null)
            {
                if (car.ImageFile.ContentType != "image/jpeg" && car.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View();
                }
                if (car.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View();
                }
                string fileName = car.ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                newFileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    car.ImageFile.CopyTo(stream);
                }
            }
            if (newFileName != null)
            {
                string deletePath = Path.Combine(_env.WebRootPath, "assets", "images", existCar.Image);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existCar.Image = newFileName;
            }

            ViewBag.Brands = _context.Brands.ToList();
            existCar.BrandId = car.BrandId;
            existCar.Model = car.Model;
            existCar.Doors = car.Doors;
            existCar.Seats = car.Seats;
            existCar.Price = car.Price;
            existCar.Luggage = car.Luggage;
            existCar.Transmission = car.Transmission;
            existCar.AirConditioning = car.AirConditioning;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            var car = _context.Cars.FirstOrDefault(x => x.Id == id);
            _context.Cars.Remove(car);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
