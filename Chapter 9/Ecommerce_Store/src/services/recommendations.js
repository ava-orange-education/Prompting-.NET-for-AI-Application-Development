const allRecommendations = [
  { id: 101, name: "Monitor", price: 450, image: "https://placehold.co/300x200/f59e0b/ffffff?text=Monitor", category: "Monitors" },
  { id: 102, name: "Wireless Mouse", price: 80, image: "https://placehold.co/300x200/ec4899/ffffff?text=Wireless+Mouse", category: "Mice" },
  { id: 103, name: "Laptop Stand", price: 45, image: "https://placehold.co/300x200/06b6d4/ffffff?text=Laptop+Stand", category: "Accessories" },
  { id: 104, name: "USB-C Hub", price: 35, image: "https://placehold.co/300x200/d946ef/ffffff?text=USB-C+Hub", category: "Accessories" },
  { id: 105, name: "Mechanical Keyboard", price: 120, image: "https://placehold.co/300x200/10b981/ffffff?text=Mechanical+KB", category: "Keyboards" },
  { id: 106, name: "Noise-Canceling Headphones", price: 250, image: "https://placehold.co/300x200/f97316/ffffff?text=ANC+Headphones", category: "Headphones" },
];

const getRecommendations = async (cartItems) => {
  const categories = cartItems.map((item) => item.category);
  const recommended = allRecommendations.filter(
    (r) => !categories.includes(r.category)
  );
  await new Promise((r) => setTimeout(r, 600));
  return recommended.slice(0, 3);
};

export default getRecommendations;
