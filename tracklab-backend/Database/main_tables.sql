-- Main tables and relationships for TrackLab

CREATE TABLE tenants (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    ruc NVARCHAR(11) NOT NULL,
    legal_name NVARCHAR(200) NOT NULL,
    code NVARCHAR(20) NOT NULL,
    commercial_name NVARCHAR(100),
    address NVARCHAR(200),
    city NVARCHAR(50),
    country NVARCHAR(50),
    phone_number NVARCHAR(20),
    email NVARCHAR(100),
    website NVARCHAR(100),
    active BIT NOT NULL
);

CREATE TABLE users (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) NOT NULL,
    password_hash NVARCHAR(120) NOT NULL,
    email NVARCHAR(100) NOT NULL,
    first_name NVARCHAR(50),
    last_name NVARCHAR(50),
    active BIT NOT NULL,
    tenant_id BIGINT NOT NULL,
    CONSTRAINT FK_users_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

CREATE TABLE vehicles (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    tenant_id BIGINT NOT NULL,
    license_plate NVARCHAR(20) NOT NULL,
    load_capacity DECIMAL(18,2) NOT NULL,
    pax_capacity INT NOT NULL,
    status INT NOT NULL,
    latitude FLOAT NOT NULL,
    longitude FLOAT NOT NULL,
    -- image_asset_ids: handled as a separate table if needed
    CONSTRAINT FK_vehicles_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

CREATE TABLE warehouses (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    tenant_id BIGINT NOT NULL,
    name NVARCHAR(100) NOT NULL,
    type INT NOT NULL,
    latitude FLOAT NOT NULL,
    longitude FLOAT NOT NULL,
    address NVARCHAR(200) NOT NULL,
    CONSTRAINT FK_warehouses_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

CREATE TABLE products (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    tenant_id BIGINT NOT NULL,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(200) NOT NULL,
    price_amount DECIMAL(18,2) NOT NULL,
    price_currency NVARCHAR(10) NOT NULL,
    CONSTRAINT FK_products_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

CREATE TABLE positions (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    tenant_id BIGINT NOT NULL,
    name NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_positions_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

CREATE TABLE employees (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    tenant_id BIGINT NOT NULL,
    dni NVARCHAR(20) NOT NULL,
    email NVARCHAR(100) NOT NULL,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    phone_number NVARCHAR(20) NOT NULL,
    position_id BIGINT NOT NULL,
    status INT NOT NULL,
    CONSTRAINT FK_employees_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id),
    CONSTRAINT FK_employees_position FOREIGN KEY (position_id) REFERENCES positions(id)
);

CREATE TABLE orders (
    order_id BIGINT PRIMARY KEY IDENTITY(1,1),
    tenant_id BIGINT NOT NULL,
    logistics_id BIGINT NOT NULL,
    shipping_address NVARCHAR(200) NOT NULL,
    order_date DATETIME NOT NULL,
    status INT NOT NULL,
    CONSTRAINT FK_orders_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id),
    CONSTRAINT FK_orders_logistics FOREIGN KEY (logistics_id) REFERENCES tenants(id)
);

CREATE TABLE order_items (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    order_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,
    quantity INT NOT NULL,
    price_amount DECIMAL(18,2) NOT NULL,
    price_currency NVARCHAR(10) NOT NULL,
    CONSTRAINT FK_order_items_order FOREIGN KEY (order_id) REFERENCES orders(order_id),
    CONSTRAINT FK_order_items_product FOREIGN KEY (product_id) REFERENCES products(id)
);

CREATE TABLE routes (
    route_id BIGINT PRIMARY KEY IDENTITY(1,1),
    vehicle_id BIGINT NOT NULL,
    CONSTRAINT FK_routes_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicles(id)
);

CREATE TABLE routes_orders (
    route_id BIGINT NOT NULL,
    order_id BIGINT NOT NULL,
    PRIMARY KEY (route_id, order_id),
    CONSTRAINT FK_routes_orders_route FOREIGN KEY (route_id) REFERENCES routes(route_id),
    CONSTRAINT FK_routes_orders_order FOREIGN KEY (order_id) REFERENCES orders(order_id)
);

CREATE TABLE route_items (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    route_id BIGINT NOT NULL,
    warehouse_id BIGINT NOT NULL,
    completed_at DATETIME,
    is_completed BIT NOT NULL,
    CONSTRAINT FK_route_items_route FOREIGN KEY (route_id) REFERENCES routes(route_id),
    CONSTRAINT FK_route_items_warehouse FOREIGN KEY (warehouse_id) REFERENCES warehouses(id)
);

CREATE TABLE containers (
    container_id BIGINT PRIMARY KEY IDENTITY(1,1),
    order_id BIGINT NOT NULL,
    warehouse_id BIGINT NOT NULL,
    CONSTRAINT FK_containers_order FOREIGN KEY (order_id) REFERENCES orders(order_id),
    CONSTRAINT FK_containers_warehouse FOREIGN KEY (warehouse_id) REFERENCES warehouses(id)
);

CREATE TABLE ship_items (
    container_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,
    quantity INT NOT NULL,
    PRIMARY KEY (container_id, product_id),
    CONSTRAINT FK_ship_items_container FOREIGN KEY (container_id) REFERENCES containers(container_id),
    CONSTRAINT FK_ship_items_product FOREIGN KEY (product_id) REFERENCES products(id)
);

CREATE TABLE tracking_events (
    event_id BIGINT PRIMARY KEY IDENTITY(1,1),
    container_id BIGINT NOT NULL,
    warehouse_id BIGINT NOT NULL,
    type INT NOT NULL,
    event_time DATETIME NOT NULL,
    CONSTRAINT FK_tracking_events_container FOREIGN KEY (container_id) REFERENCES containers(container_id),
    CONSTRAINT FK_tracking_events_warehouse FOREIGN KEY (warehouse_id) REFERENCES warehouses(id)
); 