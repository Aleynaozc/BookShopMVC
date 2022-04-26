using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopMvc.Models;
using BookShopMvc.Data;
using Microsoft.EntityFrameworkCore;
using BookShopMvc.ViewModals;

namespace BookShopMvc.Controllers
{
    public class BookController : Controller
    {
        //context sayesinde veritabanıyla iletişim sağlanır.
        private readonly BookContext _context;
        public BookController(BookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            BookNCategoryNSeller bookNCategoryN = new BookNCategoryNSeller();

            bookNCategoryN.Books = _context.Books.ToList();
            bookNCategoryN.Categories = _context.Categories.ToList();
            bookNCategoryN.Sellers = _context.Sellers.ToList();

            bookNCategoryN.Categories = _context.CategoryBooks
                .Include(c => c.Category)
                 .Select(bm => bm.Category).ToList();

            bookNCategoryN.Sellers = _context.SellerBooks
                .Include(c => c.Seller)
                 .Select(bm => bm.Seller).ToList();


            return View(bookNCategoryN);

           
            
            //return View(bookNCategoryN); //DB'den bilgiler alınarak listelendi.
        }
        public IActionResult Detail(int id)
        {
            var bookAndComments = _context.Books
                                   .Where(m => m.ID == id)
                                   .Include(m => m.Comments)
                                   .Select(m =>
                                       new Book()
                                       {
                                           ID = m.ID,
                                           Name = m.Name,
                                           Author = m.Author,
                                           ImgUrl = m.ImgUrl,
                                           Price = m.Price,
                                           Comments = m.Comments
                                       })
                                   .FirstOrDefault();

            var Categories = _context.CategoryBooks
                .Where (c=>c.Book.ID==id)
                .Include(c => c.Category)
                 .Select(bm => bm.Category).ToList();

            var Sellers = _context.SellerBooks
                .Where(c => c.Book.ID == id)
                .Include(s => s.Seller)
                 .Select(bm => bm.Seller).ToList();

            ViewBag.Category= Categories;
            ViewBag.Sellers = Sellers;
            ViewBag.Book = bookAndComments;

            return View();
        }

        [HttpPost]
        public IActionResult SaveComment(SaveCommentViewModel saveComment)
        {
            Comment comment = new Comment();
            comment.Text = saveComment.Comment;
            comment.Book = _context.Books.SingleOrDefault(b => b.ID == saveComment.BookID);
            if (comment.Text != null)
            {
                _context.Comments.Add(comment);
            }


            _context.SaveChanges();
            return RedirectToAction("Detail", new { Id = saveComment.BookID });
        }

        public IActionResult DeleteComment(Comment comment)
        {
            var deletedComment = _context.Comments
                                       .Where(b => b.ID == comment.ID)
                                       .Include(c => c.Book)
                                       .FirstOrDefault();
            var ID = deletedComment.Book.ID;
            _context.Comments.Remove(deletedComment);
            _context.SaveChanges();
            return RedirectToAction("Detail", new { ID });

        }



        public IActionResult Create()
        {
            CategorySellerBook categorySellerBook = new CategorySellerBook();

            var categories = _context.Categories.ToList();
            var sellers = _context.Sellers.ToList();
            ViewBag.Categories = categories;
            ViewBag.Seller = sellers;
            return View(categorySellerBook);
        }

        [HttpPost]
        public ActionResult Create(CategorySellerBook newBook)
        {
                 Book book = new Book();
                 
                 book.Name = newBook.Name;
                 book.Author = newBook.Author;
                 book.Price = newBook.Price;
                 book.ImgUrl = newBook.ImgUrl;

                _context.Books.Add(book);

            newBook.CategoryIds.ForEach(category =>
            {
                CategoryBook categoryBook = new CategoryBook();
                categoryBook.Book = book;
                categoryBook.Category=_context.Categories.FirstOrDefault(c => c.Id == category);
                _context.CategoryBooks.Add(categoryBook);
            });

            newBook.SellerIds.ForEach(seller =>
            {
                SellerBook SellerBook = new SellerBook();
                SellerBook.Book = book;
                SellerBook.Seller = _context.Sellers.FirstOrDefault(s => s.Id == seller);
                _context.SellerBooks.Add(SellerBook);
            });



            _context.SaveChanges();
                return RedirectToAction("Index");
           

        }
        public IActionResult Delete(int id)
        {


            var deletedCategoryBook = _context.CategoryBooks.
                 Where(c => c.Book.ID == id);
                 _context.CategoryBooks.RemoveRange(deletedCategoryBook);

            var deletedSellerBook = _context.SellerBooks.
                Where(s => s.Book.ID == id);
            _context.SellerBooks.RemoveRange(deletedSellerBook);
           
            var deletedCommentBook = _context.Comments.
                Where(z => z.Book.ID == id);
            _context.Comments.RemoveRange(deletedCommentBook);


            var deletedBook = _context.Books
                                       .SingleOrDefault(b => b.ID == id);
            _context.Books.Remove(deletedBook);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }


        public IActionResult Update(int id)
        {
            var UpdatedBook = _context.Books.SingleOrDefault(u => u.ID == id);//SingleOrDefault=Idleri dönüp buluyor.

            CategorySellerBook categorySellerBook = new CategorySellerBook();
            categorySellerBook.ID = UpdatedBook.ID;
            categorySellerBook.Name = UpdatedBook.Name;
            categorySellerBook.Author = UpdatedBook.Author;
            categorySellerBook.Price = UpdatedBook.Price;
            categorySellerBook.ImgUrl = UpdatedBook.ImgUrl;

            categorySellerBook.CategoryIds = _context.CategoryBooks
                .Where(b => b.Book.ID == id)
                .Include(c => c.Category)
                .Select(d => d.Category.Id).ToList();
            
            categorySellerBook.SellerIds = _context.SellerBooks
               .Where(b => b.Book.ID == id)
               .Include(c => c.Seller)
               .Select(d => d.Seller.Id).ToList();

            var Categories = _context.Categories.ToList();
            var Sellers = _context.Sellers.ToList();
            ViewBag.Categories = Categories;
            ViewBag.Seller = Sellers;

            ViewBag.Book = UpdatedBook;
            return View(categorySellerBook);
            
        }


        [HttpPost]
        public ActionResult Update(CategorySellerBook book)
        {
            var UpdatedBook = _context.Books.SingleOrDefault(u => u.ID == book.ID);//SingleOrDefault=Idleri dönüp buluyor.
            UpdatedBook.ID = book.ID;
            UpdatedBook.Name = book.Name;
            UpdatedBook.ImgUrl = book.ImgUrl;
            UpdatedBook.Author = book.Author;
            UpdatedBook.Price = book.Price;

            var categoryList = _context.CategoryBooks
                .Where(cl => cl.Book.ID == book.ID)
                .Select(cl => cl.Category.Id).ToList();
           
            var deletedCategory = categoryList.Except(book.CategoryIds).ToList();

            deletedCategory.ForEach (item =>
            {
                var deletedBookCategory = _context.CategoryBooks.FirstOrDefault(c => c.Category.Id == item);
                var delete = _context.CategoryBooks.Remove(deletedBookCategory);//Bir önce eklenen kategoriyi siliyor.

            }) ;
            _context.SaveChanges();

            var addedCategory = book.CategoryIds.Except(categoryList).ToList();
            addedCategory.ForEach(category =>
            {
                CategoryBook categoryBook = new CategoryBook();
                categoryBook.Book = UpdatedBook;
                categoryBook.Category = _context.Categories.FirstOrDefault(c => c.Id == category);
                _context.CategoryBooks.Add(categoryBook);
            });

            var sellerList = _context.SellerBooks
                .Where(sb => sb.Book.ID == book.ID)
                .Select(sb => sb.Seller.Id).ToList();
            var deletedSeller = sellerList.Except(book.SellerIds).ToList();
           deletedSeller.ForEach(item =>
            {
                var deletedBookSeller = _context.SellerBooks.FirstOrDefault(s => s.Seller.Id == item);
                var delete = _context.SellerBooks.Remove(deletedBookSeller);
            });
            _context.SaveChanges();

            var addedSeller = book.SellerIds.Except(sellerList).ToList();
            addedSeller.ForEach(seller =>
            {
                SellerBook sellerBook = new SellerBook();
                sellerBook.Book = UpdatedBook;
                sellerBook.Seller = _context.Sellers.FirstOrDefault(s => s.Id == seller);
                _context.SellerBooks.Add(sellerBook);
            });
            _context.SaveChanges();
            _context.Books.Update(UpdatedBook);


            
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult CreateCategory(Category category)
        //{

        //    _context.Categories.Add(category);
        //    _context.SaveChanges();

        //    return RedirectToAction("CreateCategoryNSeller");
        //}
        //[HttpPost]
        //public ActionResult CreateSeller(Seller seller)
        //{

        //    _context.Sellers.Add(seller);
        //    _context.SaveChanges();

        //    return RedirectToAction("CreateCategoryNSeller");
        //}
        //public ActionResult CreateCategoryNSeller()
        //{

        //    return View("CreateCategoryNSeller");
        //}

        [HttpPost]
        public Category SaveCategory( Category category)
        {
            var addedCat = _context.Add(category);
            return addedCat;
        }
        public async Task<List<Category>> GetCategories()
        {
            var categoryList = await _categoryService.GetList();
            return categoryList;
        }

    }


}
    

