export async function fetchProducts() {
  const res = await fetch("/api/products");
  if (!res.ok) throw new Error("Failed to fetch products");
  return res.json();
}

export async function createOrder(items) {
  const body = {
    items: items.map((item) => ({
      productId: item.id,
      productName: item.name,
      unitPrice: item.price,
      quantity: item.quantity,
    })),
  };
  const res = await fetch("/api/orders", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  if (!res.ok) {
    const errorText = await res.text();
    throw new Error(errorText || "Failed to create order");
  }
  return res.json();
}
