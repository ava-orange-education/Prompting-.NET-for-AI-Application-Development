import { useState, useEffect } from "react";
import { createOrder } from "../services/api";

export default function CheckoutModal({ cartItems, total, onClose }) {
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;
    createOrder(cartItems)
      .then((data) => {
        if (!cancelled) {
          setOrder(data);
          setLoading(false);
        }
      })
      .catch((err) => {
        if (!cancelled) {
          setError(err.message);
          setLoading(false);
        }
      });
    return () => { cancelled = true; };
  }, [cartItems]);

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center px-4">
      <div className="absolute inset-0 bg-black/40" onClick={onClose} />
      <div className="relative bg-white rounded-xl shadow-2xl max-w-md w-full p-6 animate-fade-in">
        {loading && (
          <div className="text-center py-8">
            <div className="w-12 h-12 border-4 border-blue-200 border-t-blue-600 rounded-full animate-spin mx-auto mb-4" />
            <p className="text-gray-500">Processing your order...</p>
          </div>
        )}

        {error && (
          <div className="text-center py-8">
            <div className="w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg className="w-8 h-8 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </div>
            <h2 className="text-xl font-bold text-gray-900 mb-2">Order Failed</h2>
            <p className="text-gray-500 mb-6">{error}</p>
            <button
              onClick={onClose}
              className="w-full px-4 py-2.5 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 transition-colors"
            >
              Try Again
            </button>
          </div>
        )}

        {order && (
          <div className="text-center">
            <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
            </div>
            <h2 className="text-2xl font-bold text-gray-900 mb-2">Purchase Completed Successfully</h2>
            <p className="text-gray-500 mb-6">Thank you for your order!</p>

            <div className="bg-gray-50 rounded-lg p-4 mb-6 text-left">
              <p className="text-sm font-medium text-gray-700 mb-2">Order Summary</p>
              <div className="space-y-1.5 text-sm">
                <div className="flex justify-between text-gray-600">
                  <span>Order ID</span>
                  <span className="font-mono font-medium text-gray-900">{order.orderId}</span>
                </div>
                <div className="flex justify-between text-gray-600">
                  <span>Status</span>
                  <span className="font-medium text-green-600">{order.status}</span>
                </div>
                <div className="flex justify-between text-gray-600">
                  <span>Items</span>
                  <span className="text-gray-900">{cartItems.reduce((s, i) => s + i.quantity, 0)}</span>
                </div>
                {order.createdAt && (
                  <div className="flex justify-between text-gray-600">
                    <span>Date</span>
                    <span className="text-gray-900">{new Date(order.createdAt).toLocaleDateString()}</span>
                  </div>
                )}
                <div className="border-t border-gray-200 pt-1.5 mt-1.5 flex justify-between font-bold text-gray-900">
                  <span>Total</span>
                  <span>${(order.totalAmount ?? total).toFixed(2)}</span>
                </div>
              </div>
            </div>

            <button
              onClick={onClose}
              className="w-full px-4 py-2.5 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 transition-colors"
            >
              Continue Shopping
            </button>
          </div>
        )}
      </div>
    </div>
  );
}
