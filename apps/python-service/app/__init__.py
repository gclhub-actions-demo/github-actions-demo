"""Flask microservice application."""

from flask import Flask, jsonify, request
from datetime import datetime
import os

app = Flask(__name__)


@app.route('/health', methods=['GET'])
def health():
    """Health check endpoint."""
    return jsonify({
        'status': 'healthy',
        'timestamp': datetime.utcnow().isoformat(),
        'version': '1.0.0'
    }), 200


@app.route('/api/products', methods=['GET'])
def get_products():
    """Get all products."""
    products = [
        {'id': 1, 'name': 'Laptop', 'price': 999.99, 'stock': 50},
        {'id': 2, 'name': 'Mouse', 'price': 29.99, 'stock': 200},
        {'id': 3, 'name': 'Keyboard', 'price': 79.99, 'stock': 150},
    ]
    return jsonify({'products': products}), 200


@app.route('/api/products/<int:product_id>', methods=['GET'])
def get_product(product_id):
    """Get a specific product by ID."""
    products = [
        {'id': 1, 'name': 'Laptop', 'price': 999.99, 'stock': 50},
        {'id': 2, 'name': 'Mouse', 'price': 29.99, 'stock': 200},
        {'id': 3, 'name': 'Keyboard', 'price': 79.99, 'stock': 150},
    ]

    product = next((p for p in products if p['id'] == product_id), None)

    if product:
        return jsonify(product), 200
    return jsonify({'error': 'Product not found'}), 404


@app.route('/api/products', methods=['POST'])
def create_product():
    """Create a new product."""
    data = request.get_json()

    if not data or 'name' not in data or 'price' not in data:
        return jsonify({'error': 'Name and price are required'}), 400

    new_product = {
        'id': int(datetime.utcnow().timestamp()),
        'name': data['name'],
        'price': data['price'],
        'stock': data.get('stock', 0)
    }

    return jsonify(new_product), 201


@app.errorhandler(404)
def not_found(error):
    """Handle 404 errors."""
    return jsonify({'error': 'Not found'}), 404


@app.errorhandler(500)
def internal_error(error):
    """Handle 500 errors."""
    return jsonify({'error': 'Internal server error'}), 500


if __name__ == '__main__':
    port = int(os.environ.get('PORT', 5000))
    app.run(host='0.0.0.0', port=port, debug=os.environ.get('DEBUG', False))
