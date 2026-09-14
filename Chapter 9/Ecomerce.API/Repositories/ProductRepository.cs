using Ecomerce.API.Models;

namespace Ecomerce.API.Repositories
{
    public static class ProductRepository
    {
        private static readonly List<Product> _products =
        [
            new()
            {
                Id = 1,
                Name = "Laptop",
                Description = "High-quality over-ear headphones with noise cancellation.",
                Price = 79.99m,
                Category = "Electronics",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\">\n  <rect width=\"120\" height=\"90\" fill=\"#f3f4f6\"/>\n  <rect x=\"25\" y=\"15\" width=\"70\" height=\"40\" rx=\"3\" fill=\"#374151\"/>\n  <rect x=\"30\" y=\"20\" width=\"60\" height=\"30\" fill=\"#60a5fa\"/>\n  <rect x=\"20\" y=\"60\" width=\"80\" height=\"8\" rx=\"2\" fill=\"#6b7280\"/>\n</svg>"
            },
           new()
            {
                Id = 2,
                Name = "Organic Green Tea",
                Description = "Premium loose-leaf green tea from Japan.",
                Price = 12.49m,
                Category = "Groceries",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"120\" height=\"90\" fill=\"#f0fdf4\"/><rect x=\"45\" y=\"20\" width=\"30\" height=\"40\" rx=\"3\" fill=\"#15803d\"/><rect x=\"40\" y=\"15\" width=\"40\" height=\"10\" rx=\"2\" fill=\"#22c55e\"/><ellipse cx=\"60\" cy=\"65\" rx=\"20\" ry=\"8\" fill=\"#86efac\"/></svg>"
            },
            new()
            {
                Id = 3,
                Name = "Leather Messenger Bag",
                Description = "Handcrafted genuine leather bag perfect for work or travel.",
                Price = 149.00m,
                Category = "Accessories",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"120\" height=\"90\" fill=\"#fef3c7\"/><rect x=\"25\" y=\"30\" width=\"70\" height=\"40\" rx=\"4\" fill=\"#92400e\"/><path d=\"M40 30 C40 10,80 10,80 30\" fill=\"none\" stroke=\"#78350f\" stroke-width=\"4\"/></svg>"
            },
            new()
            {
                Id = 4,
                Name = "Running Shoes",
                Description = "Lightweight and cushioned shoes for everyday running.",
                Price = 89.99m,
                Category = "Sports",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"120\" height=\"90\" fill=\"#eff6ff\"/><path d=\"M20 55 L45 40 L70 50 L95 55 L100 65 L20 65 Z\" fill=\"#2563eb\"/><rect x=\"30\" y=\"45\" width=\"25\" height=\"5\" fill=\"#ffffff\"/></svg>"
            },
            new()
            {
                Id = 5,
                Name = "Smart Water Bottle",
                Description = "Temperature-displaying bottle with hydration reminder.",
                Price = 24.99m,
                Category = "Lifestyle",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"120\" height=\"90\" fill=\"#ecfeff\"/><rect x=\"45\" y=\"15\" width=\"30\" height=\"55\" rx=\"8\" fill=\"#0891b2\"/><rect x=\"50\" y=\"10\" width=\"20\" height=\"10\" rx=\"2\" fill=\"#155e75\"/><circle cx=\"60\" cy=\"45\" r=\"8\" fill=\"#ffffff\"/></svg>"
            },
            new()
            {
                Id = 6,
                Name = "Organic Green Tea",
                Description = "Premium loose-leaf green tea from Japan.",
                Price = 12.49m,
                Category = "Groceries",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"120\" height=\"90\" fill=\"#f0fdf4\"/><rect x=\"45\" y=\"20\" width=\"30\" height=\"40\" rx=\"3\" fill=\"#15803d\"/><rect x=\"40\" y=\"15\" width=\"40\" height=\"10\" rx=\"2\" fill=\"#22c55e\"/><ellipse cx=\"60\" cy=\"65\" rx=\"20\" ry=\"8\" fill=\"#86efac\"/></svg>"
            },
            new()
            {
                Id = 7,
                Name = "Leather Messenger Bag",
                Description = "Handcrafted genuine leather bag perfect for work or travel.",
                Price = 149.00m,
                Category = "Accessories",
                Image = "<svg viewBox=\"0 0 120 90\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"120\" height=\"90\" fill=\"#fef3c7\"/><rect x=\"25\" y=\"30\" width=\"70\" height=\"40\" rx=\"4\" fill=\"#92400e\"/><path d=\"M40 30 C40 10,80 10,80 30\" fill=\"none\" stroke=\"#78350f\" stroke-width=\"4\"/></svg>"
            }
        ];

        public static List<Product> GetAll() => _products;
    }
}
