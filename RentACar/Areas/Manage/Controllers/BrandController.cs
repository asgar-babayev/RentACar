using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.DAL;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Manage.Controllers
{
    [Area("Manage"), Authorize]
    public class BrandController : Controller
    {
        private readonly Context _context;

        public BrandController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Brands.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid) return View(brand);
            if (_context.Brands.Any(x => x.Name.Trim().ToLower() == brand.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "This brand already have");
                return View(brand);
            }
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Edit(Brand brand)
        {
            var existbrand = _context.Brands.FirstOrDefault(x => x.Id == brand.Id);
            if (existbrand == null) ModelState.AddModelError("Error", "Brand can't empty");
            if (_context.Brands.Any(x => x.Name.Trim().ToLower() == brand.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "This brand already have");
                return View(brand);
            }
            existbrand.Name = brand.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.FirstOrDefault(x => x.Id == id);
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
