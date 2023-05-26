SELECT eliminartabla('tb_categorias');

CREATE TABLE tb_categorias(
	id SERIAL,
	numero_categoria INTEGER NOT NULL,
	nombre_categoria VARCHAR(50) NOT NULL,
	folio INTEGER NOT NULL DEFAULT 0,
	status SMALLINT NOT NULL DEFAULT 0,
	fecha_alta DATE NOT NULL DEFAULT CURRENT_DATE,
	PRIMARY KEY (numero_categoria, nombre_categoria)
);
