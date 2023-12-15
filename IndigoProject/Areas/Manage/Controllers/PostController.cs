using IndigoProject.Areas.Manage.ViewModels;
using IndigoProject.DAL;
using IndigoProject.Helper;
using IndigoProject.Models;
using IndigoProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndigoProject.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PostController : Controller
    {
        AppDBC _context;
        private readonly IWebHostEnvironment _env;

        public PostController(AppDBC context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Posts = await _context.posts.ToListAsync()
            };


            return View(homeVM);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM createPostVM)
        {
            if (createPostVM is null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!createPostVM.Image.CheckContent("image/"))
            {
                ModelState.AddModelError("Image", "Duzgun format daxil edin");
                return View();
            }
            Post post = new Post()
            {
                Title= createPostVM.Title,
                Description=createPostVM.Description,
                ImageUrl = createPostVM.Image.UploadFile(_env.WebRootPath, "/Upload/Post/")
            };
            await _context.posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {

            Post post = _context.posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();

            
                post.ImageUrl.RemoveFile(_env.WebRootPath, @"\Upload\Product\");
          
            _context.posts.Remove(post);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Post post = await _context.posts.Where(b => b.Id == id).FirstOrDefaultAsync();
            UpdatePostVM updatePostVM = new UpdatePostVM()
            {
                Id = id,
                Title = post.Title,
                Description = post.Description,
                ImageUrl = post.ImageUrl
            };


            return View(updatePostVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostVM updatePostVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!updatePostVM.Image.CheckContent("image/"))
            {
                ModelState.AddModelError("Image", "Duzgun format daxil edin");
                return View();
            }
            Post post = _context.posts.Find(updatePostVM.Id);
            post.Title=updatePostVM.Title;
            post.Description = updatePostVM.Description;
            post.ImageUrl = updatePostVM.Image.UploadFile(_env.WebRootPath, "/Upload/Post/");

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
