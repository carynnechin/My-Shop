﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System.IO;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        // GET: ProductManager
        public ProductManagerController(IRepository<Product> Productcontext, IRepository<ProductCategory> productCategoryContext )
        {
            context = Productcontext;
            productCategories = productCategoryContext;
        }

        public ActionResult Index()
        {

            List<Product> products = context.Collection().ToList();
            return View(products);
        }
       
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {

            if (!ModelState.IsValid)
            {
                return View(product);
            }
             else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//")+ product.Image);

                }
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
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();
                return View(viewModel);
            }
                       
        }
        [HttpPost]
        public ActionResult Edit(Product product ,String Id, HttpPostedFileBase file)
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

                if (file!= null)
                {
                    ProductToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + ProductToEdit.Image);
                }

                ProductToEdit.Category = product.Category;
                ProductToEdit.Category = product.Description;
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