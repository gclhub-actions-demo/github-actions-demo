"""Tests for Flask microservice."""

import pytest
from app import app as flask_app


@pytest.fixture
def app():
    """Create application fixture."""
    flask_app.config['TESTING'] = True
    yield flask_app


@pytest.fixture
def client(app):
    """Create test client."""
    return app.test_client()


class TestHealthEndpoint:
    """Test health check endpoint."""

    def test_health_check(self, client):
        """Test health endpoint returns 200."""
        response = client.get('/health')
        assert response.status_code == 200
        data = response.get_json()
        assert data['status'] == 'healthy'
        assert 'timestamp' in data
        assert 'version' in data


class TestProductsEndpoint:
    """Test products endpoints."""

    def test_get_products(self, client):
        """Test getting all products."""
        response = client.get('/api/products')
        assert response.status_code == 200
        data = response.get_json()
        assert 'products' in data
        assert isinstance(data['products'], list)
        assert len(data['products']) > 0

    def test_get_product_by_id(self, client):
        """Test getting a specific product."""
        response = client.get('/api/products/1')
        assert response.status_code == 200
        data = response.get_json()
        assert data['id'] == 1
        assert 'name' in data
        assert 'price' in data
        assert 'stock' in data

    def test_get_product_not_found(self, client):
        """Test getting non-existent product."""
        response = client.get('/api/products/999')
        assert response.status_code == 404
        data = response.get_json()
        assert 'error' in data

    def test_create_product(self, client):
        """Test creating a new product."""
        new_product = {
            'name': 'Monitor',
            'price': 299.99,
            'stock': 75
        }
        response = client.post('/api/products', json=new_product)
        assert response.status_code == 201
        data = response.get_json()
        assert 'id' in data
        assert data['name'] == new_product['name']
        assert data['price'] == new_product['price']
        assert data['stock'] == new_product['stock']

    def test_create_product_missing_name(self, client):
        """Test creating product without name."""
        response = client.post('/api/products', json={'price': 99.99})
        assert response.status_code == 400
        data = response.get_json()
        assert 'error' in data

    def test_create_product_missing_price(self, client):
        """Test creating product without price."""
        response = client.post('/api/products', json={'name': 'Test'})
        assert response.status_code == 400
        data = response.get_json()
        assert 'error' in data


class TestErrorHandlers:
    """Test error handlers."""

    def test_not_found(self, client):
        """Test 404 error handler."""
        response = client.get('/unknown-endpoint')
        assert response.status_code == 404
        data = response.get_json()
        assert data['error'] == 'Not found'
