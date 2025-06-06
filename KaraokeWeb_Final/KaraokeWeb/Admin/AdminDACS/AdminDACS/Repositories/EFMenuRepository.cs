using AdminDACS.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDACS.Repositories
{
    public class EFMenuRepository : IMenuRepository
    {
        private readonly ApplicationDBContext _context;

        public EFMenuRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> ShowAsync()
        {
            return await _context.menus.Include(m => m.NguyenLieu).ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _context.menus.Include(m => m.NguyenLieu).ToListAsync();
        }

        public async Task<Menu> GetByIdAsync(string menuId)
        {
            return await _context.menus.Include(m => m.NguyenLieu).FirstOrDefaultAsync(m => m.MaMon == menuId);
        }

        public async Task AddAsync(Menu menu)
        {
            await _context.menus.AddAsync(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Menu menu)
        {
            _context.menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = await _context.menus.FindAsync(id);
            if (item == null) return false;

            if (!string.IsNullOrEmpty(item.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.ImageUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.menus.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> AddMenuWithIngredientAsync(Menu menu, string TenNL, decimal GiaNL, int SL, IFormFile imageFile)
        {
            try
            {
                // Tạo mã món: MON1, MON2, ...
                int maxMon = 0;
                var maMons = _context.menus.AsEnumerable().Select(m =>
                {
                    if (int.TryParse(m.MaMon.Substring(3), out int n))
                        return n;
                    else
                        return 0;
                });
                if (maMons.Any())
                    maxMon = maMons.Max();

                menu.MaMon = "MON" + (maxMon + 1);

                // Tạo mã nguyên liệu: NL1, NL2, ...
                int maxNL = 0;
                var maNLs = _context.Warehouses.AsEnumerable().Select(nl =>
                {
                    if (int.TryParse(nl.MaNL.Substring(2), out int n))
                        return n;
                    else
                        return 0;
                });
                if (maNLs.Any())
                    maxNL = maNLs.Max();

                string newMaNL = "NL" + (maxNL + 1);
                menu.MaNL = newMaNL;

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageFood");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    string fileName = Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    menu.ImageUrl = "ImageFood/" + fileName;
                }

                var nguyenLieu = new Warehouse
                {
                    MaNL = newMaNL,
                    TenNL = TenNL,
                    Gia = GiaNL,
                    SL = SL,
                    MaMon = menu.MaMon
                };

                menu.NguyenLieu = nguyenLieu;

                var materialInput = new MaterialInput
                {
                    MaNL = TenNL,
                    Quantity = SL,
                    UnitPrice = GiaNL
                };

                _context.MaterialInputs.Add(materialInput);
                _context.Warehouses.Add(nguyenLieu);
                _context.menus.Add(menu);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
