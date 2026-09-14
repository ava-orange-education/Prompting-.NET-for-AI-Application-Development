import { useState, useEffect, useCallback, useMemo } from "react";
import { fetchProducts } from "./services/api";
import getRecommendations from "./services/recommendations";
import Header from "./components/Header";
import ProductCatalog from "./components/ProductCatalog";
import ShoppingCart from "./components/ShoppingCart";
import RecommendationPanel from "./components/RecommendationPanel";
import CheckoutModal from "./components/CheckoutModal";

export default function App() {
  const [products, setProducts] = useState([]);
  const [productsLoading, setProductsLoading] = useState(true);
  const [productsError, setProductsError] = useState(null);
  const [cartItems, setCartItems] = useState([]);
  const [cartOpen, setCartOpen] = useState(false);
  const [checkoutOpen, setCheckoutOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [recommendations, setRecommendations] = useState([]);
  const [recLoading, setRecLoading] = useState(false);

  const filteredProducts = useMemo(
    () => products.filter(
      (p) =>
        p.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        p.description.toLowerCase().includes(searchQuery.toLowerCase()) ||
        p.category.toLowerCase().includes(searchQuery.toLowerCase())
    ),
    [searchQuery, products]
  );

  const addToCart = useCallback((product) => {
    setCartItems((prev) => {
      const existing = prev.find((item) => item.id === product.id);
      if (existing) {
        return prev.map((item) =>
          item.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
        );
      }
      return [...prev, { ...product, quantity: 1 }];
    });
  }, []);

  const removeFromCart = useCallback((id) => {
    setCartItems((prev) => prev.filter((item) => item.id !== id));
  }, []);

  const cartCount = useMemo(
    () => cartItems.reduce((sum, item) => sum + item.quantity, 0),
    [cartItems]
  );

  const cartTotal = useMemo(
    () => cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0),
    [cartItems]
  );

  const handleCheckout = useCallback(() => {
    setCartOpen(false);
    setCheckoutOpen(true);
  }, []);

  const handleCloseCheckout = useCallback(() => {
    setCheckoutOpen(false);
    setCartItems([]);
  }, []);

  useEffect(() => {
    let cancelled = false;
    fetchProducts()
      .then((data) => {
        if (!cancelled) {
          setProducts(data);
          setProductsLoading(false);
        }
      })
      .catch((err) => {
        if (!cancelled) {
          setProductsError(err.message);
          setProductsLoading(false);
        }
      });
    return () => { cancelled = true; };
  }, []);

  useEffect(() => {
    if (cartItems.length === 0) {
      setRecommendations([]);
      return;
    }
    let cancelled = false;
    setRecLoading(true);
    getRecommendations(cartItems).then((recs) => {
      if (!cancelled) {
        setRecommendations(recs);
        setRecLoading(false);
      }
    });
    return () => {
      cancelled = true;
    };
  }, [cartItems]);

  return (
    <div className="min-h-screen bg-gray-50">
      <Header
        cartCount={cartCount}
        onCartClick={() => setCartOpen(true)}
        searchQuery={searchQuery}
        onSearchChange={setSearchQuery}
      />

      <ProductCatalog
        products={filteredProducts}
        onAddToCart={addToCart}
        loading={productsLoading}
        error={productsError}
      />

      <RecommendationPanel recommendations={recommendations} loading={recLoading} />

      {cartOpen && (
        <ShoppingCart
          cartItems={cartItems}
          onRemove={removeFromCart}
          onClose={() => setCartOpen(false)}
          onCheckout={handleCheckout}
        />
      )}

      {checkoutOpen && (
        <CheckoutModal
          cartItems={cartItems}
          total={cartTotal}
          onClose={handleCloseCheckout}
        />
      )}
    </div>
  );
}
