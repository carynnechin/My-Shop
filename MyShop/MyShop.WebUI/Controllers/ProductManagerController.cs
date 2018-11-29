﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        ProductRepository context;
        // GET: ProductManager
        public ProductManagerController()
        {
            context = new ProductRepository();
        }

        public ActionResult Index()
        {

            List<Product> products = context.Collection().ToList();
            return View(products);
        }
       
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {

            if (!ModelState.IsValid)
            {
                return View(product);
            }
             else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
      
        }

        public ActionResult Edit(String Id)
        {
            Product product = context.Find(Id);
            if (product ==null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
                       
        }
        [HttpPost]
        public ActionResult Edit(Product product ,String Id)
        {
            Product ProductToEdit = context.Find(Id);
            if (ProductToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                ProductToEdit.Category = product.Category;
                ProductToEdit.Category = product.Description;
                ProductToEdit.Image = product.Image;
                ProductToEdit.Name = product.Name;
                ProductToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Delete(string Id)
        {
            Product ProductToDelete = context.Find(Id);
            if (ProductToDelete == null)
            {
                return HttpNotFound();
            } else
            {
                return View(ProductToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product ProductToDelete = context.Find(Id);
            if (ProductToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}