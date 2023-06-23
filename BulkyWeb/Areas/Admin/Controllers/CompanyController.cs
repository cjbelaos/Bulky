using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public CompanyController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

			return View(objCompanyList);
		}

		public IActionResult Upsert(int? id)
		{
			//ViewBag.CategoryList = CategoryList; => asp-items="ViewBag.CategoryList"
			//ViewData["CategoryList"] = CategoryList => @(ViewData["CategoryList"] as IEnumerable<SelectListItem>)

			if (id == null || id == 0)
			{
				//create
				return View(new Company());
			}
			else
			{
				//update
				Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
				return View(companyObj);
			}

		}
		[HttpPost]
		public IActionResult Upsert(Company CompanyObj)
		{
			//if (obj.Name == obj.DisplayOrder.ToString())
			//{
			//    ModelState.AddModelError("name", "The Display Order cannot match the Name.");
			//}
			if (ModelState.IsValid)
			{
				if (CompanyObj.Id == 0)
				{
					_unitOfWork.Company.Add(CompanyObj);
				}
				else
				{
					_unitOfWork.Company.Update(CompanyObj);
				}

				_unitOfWork.Save();
				TempData["success"] = "Company created successfully";
				return RedirectToAction("Index");
			}
			else
			{
				return View(CompanyObj);
			}
		}

		//public IActionResult Edit(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}
		//	Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
		//	//Company? CompanyFromDb1 = _db.Companys.FirstOrDefault(u=>u.Id==id);
		//	//Company? CompanyFromDb2 = _db.Companys.Where(u=>u.Id==id).FirstOrDefault();

		//	if (CompanyFromDb == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(CompanyFromDb);
		//}
		//[HttpPost]
		//public IActionResult Edit(Company obj)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_unitOfWork.Company.Update(obj);
		//		_unitOfWork.Save();
		//		TempData["success"] = "Company updated successfully";
		//		return RedirectToAction("Index");
		//	}
		//	return View();
		//}

		//public IActionResult Delete(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}
		//	Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);

		//	if (CompanyFromDb == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(CompanyFromDb);
		//}
		//[HttpPost, ActionName("Delete")]
		//public IActionResult DeletePOST(int? id)
		//{
		//	Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
		//	if (obj == null)
		//	{
		//		return NotFound();
		//	}
		//	_unitOfWork.Company.Remove(obj);
		//	_unitOfWork.Save();
		//	TempData["success"] = "Company deleted successfully";
		//	return RedirectToAction("Index");
		//}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
			return Json(new { data = objCompanyList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
			if (CompanyToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			_unitOfWork.Company.Remove(CompanyToBeDeleted);
			_unitOfWork.Save();

			return Json(new { sucess = true, message = "Delete Successful" });
		}

		#endregion
	}
}
