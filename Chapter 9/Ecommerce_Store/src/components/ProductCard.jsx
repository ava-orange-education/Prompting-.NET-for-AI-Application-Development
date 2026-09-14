export default function ProductCard({ product, onAddToCart }) {
  return (
    <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden hover:shadow-lg hover:-translate-y-1 transition-all duration-300 flex flex-col">
      <div className="aspect-[3/2] bg-gray-100 overflow-hidden flex items-center justify-center">
      <div className="aspect-[3/2] bg-gray-100 overflow-hidden">
  <img
    src={`data:image/svg+xml;charset=utf-8,${encodeURIComponent(product.image)}`}
    alt={product.name}
    className="w-full h-full object-contain"
    loading="lazy"
  />
</div>
      </div>

      <div className="p-4 flex flex-col flex-1">
        <span className="text-xs font-medium text-blue-600 bg-blue-50 px-2 py-1 rounded-full self-start mb-2">
          {product.category}
        </span>

        <h3 className="font-semibold text-gray-900 text-base mb-1">
          {product.name}
        </h3>

        <p className="text-sm text-gray-500 mb-3 flex-1 line-clamp-2">
          {product.description}
        </p>

        <div className="flex items-center justify-between mt-auto">
          <span className="text-lg font-bold text-gray-900">
            ${product.price}
          </span>

          <button
            onClick={() => onAddToCart(product)}
            className="px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 active:scale-95 transition-all duration-150"
          >
            Add to Cart
          </button>
        </div>
      </div>
    </div>
  );
}