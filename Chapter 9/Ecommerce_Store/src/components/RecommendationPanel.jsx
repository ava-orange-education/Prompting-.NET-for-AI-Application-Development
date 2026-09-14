export default function RecommendationPanel({ recommendations, loading }) {
  if (loading) {
    return (
      <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pb-8">
        <h2 className="text-2xl font-bold text-gray-900 mb-6">Recommended For You</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {[1, 2, 3].map((i) => (
            <div key={i} className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden animate-pulse">
              <div className="aspect-[3/2] bg-gray-200" />
              <div className="p-4 space-y-3">
                <div className="h-4 bg-gray-200 rounded w-1/3" />
                <div className="h-5 bg-gray-200 rounded w-2/3" />
                <div className="h-4 bg-gray-200 rounded w-1/2" />
              </div>
            </div>
          ))}
        </div>
      </section>
    );
  }

  if (recommendations.length === 0) return null;

  return (
    <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pb-8">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Recommended For You</h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {recommendations.map((rec) => (
          <div key={rec.id} className="bg-gradient-to-br from-blue-50 to-indigo-50 rounded-xl shadow-sm border border-blue-100 overflow-hidden hover:shadow-lg hover:-translate-y-1 transition-all duration-300">
            <div className="aspect-[3/2] bg-white overflow-hidden">
              <img src={rec.image} alt={rec.name} className="w-full h-full object-cover" loading="lazy" />
            </div>
            <div className="p-4">
              <span className="text-xs font-medium text-indigo-600 bg-indigo-100 px-2 py-1 rounded-full self-start mb-2 inline-block">
                {rec.category}
              </span>
              <h3 className="font-semibold text-gray-900 text-base mb-1">{rec.name}</h3>
              <span className="text-lg font-bold text-gray-900">${rec.price}</span>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}
