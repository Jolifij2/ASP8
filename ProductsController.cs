using AjaxSecurityApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AjaxSecurityApp.Controllers
{
    public class ProductsController : Controller
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Ноутбук", Price = 999.99m, Category = "Электроника" },
            new Product { Id = 2, Name = "Книга по C#", Price = 49.99m, Category = "Книги" },
            new Product { Id = 3, Name = "Наушники", Price = 149.99m, Category = "Электроника" }
        };

        public IActionResult Index()
        {
            return View();
        }

        // AJAX: все товары
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(_products);
        }

        // AJAX: один товар
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Не найдено" });
            return Json(product);
        }

        // AJAX: добавить товар
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name))
                return BadRequest(new { message = "Некорректные данные" });

            product.Id = _products.Count + 1;
            _products.Add(product);
            return Json(new { success = true, message = $"Товар {product.Name} добавлен!", count = _products.Count });
        }

        // AJAX: удалить товар
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Не найдено" });

            _products.Remove(product);
            return Json(new { success = true, message = $"Товар {product.Name} удален!", count = _products.Count });
        }

        // Задание 1: поиск
        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(_products);

            var result = _products.Where(p => p.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase)).ToList();
            return Json(result);
        }

        // Задание 2: обновление
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromBody] Product product)
        {
            if (product == null || product.Id <= 0)
                return BadRequest(new { message = "Некорректные данные" });

            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null)
                return NotFound(new { message = "Товар не найден" });

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Category = product.Category;

            return Json(new { success = true, message = $"Товар {product.Name} обновлён!", count = _products.Count });
        }

        // Задание 5: XSS-пример (возвращает HTML с скриптом)
        [HttpGet]
        public IActionResult GetHtml()
        {
            return Content("<script>alert('XSS!')</script>", "text/html");
        }

        // Задание 5: безопасный вариант (возвращает JSON)
        [HttpGet]
        public IActionResult GetSafeHtml()
        {
            return Json(new { html = "<script>alert('XSS!')</script>" });
        }
    }
}
