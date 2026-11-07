-- =============================================================================
-- RetroGames - Script 05: SQL usado pelos repositórios (Aula 2)
-- Cada bloco é o SQL de um método do Repository, com valores reais no lugar dos
-- parâmetros (@...). Rode um bloco por vez (Ctrl+Enter em cada comando).
--
-- As gravações rodam dentro de START TRANSACTION ... ROLLBACK, então o banco
-- volta ao estado original no fim de cada teste.
-- =============================================================================

USE retrogames_beatriz;

-- =============================================================================
-- Aula 2 - Etapa 2: dados para montar a connection string
-- =============================================================================
SELECT @@hostname AS servidor, @@port AS porta, CURRENT_USER() AS usuario, DATABASE() AS banco;

-- =============================================================================
-- Aula 2 - Etapa 3: PlataformaRepository
-- =============================================================================

-- ListarAsync. Esperado: 8 plataformas em ordem alfabética
SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento
FROM tb_plataforma
ORDER BY no_plataforma;

-- BuscarPorIdAsync (@id = 5). Esperado: Super Nintendo
SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento
FROM tb_plataforma
WHERE co_seq_plataforma = 5;

-- ExisteComNomeAsync (@nome = 'NES', @idIgnorar = NULL). Esperado: 1 linha (existe)
SELECT 1 FROM tb_plataforma
WHERE no_plataforma = 'NES' AND (NULL IS NULL OR co_seq_plataforma <> NULL)
LIMIT 1;

-- ExisteComNomeAsync (@nome = 'NES', @idIgnorar = 2, ou seja, alterando a própria NES).
-- Esperado: nenhuma linha
SELECT 1 FROM tb_plataforma
WHERE no_plataforma = 'NES' AND (2 IS NULL OR co_seq_plataforma <> 2)
LIMIT 1;

-- PossuiJogosAsync (@id = 1, Atari). Esperado: 1 linha
SELECT 1 FROM tb_jogo WHERE co_plataforma = 1 LIMIT 1;

-- PossuiJogosAsync (@id = 8, Neo Geo). Esperado: nenhuma linha
SELECT 1 FROM tb_jogo WHERE co_plataforma = 8 LIMIT 1;

-- InserirAsync, AlterarAsync e ExcluirAsync (desfeitos no ROLLBACK)
START TRANSACTION;
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento)
VALUES ('Game Boy', 'Nintendo', 1989);
SELECT LAST_INSERT_ID() AS id_gerado;
UPDATE tb_plataforma
SET no_plataforma = 'Game Boy Color', no_fabricante = 'Nintendo', nu_ano_lancamento = 1998
WHERE co_seq_plataforma = LAST_INSERT_ID();
SELECT * FROM tb_plataforma WHERE co_seq_plataforma = LAST_INSERT_ID();
DELETE FROM tb_plataforma WHERE co_seq_plataforma = LAST_INSERT_ID();
ROLLBACK;

-- =============================================================================
-- Aula 2 - Etapa 4: JogoRepository
-- =============================================================================

-- ListarAsync sem filtro (@incluirInativos = FALSE, página 1, 5 por página)
-- Esperado: total 21 e os 5 primeiros jogos ativos por nome
SELECT COUNT(*) FROM tb_jogo
WHERE (FALSE = TRUE OR st_ativo = TRUE)
  AND (NULL IS NULL OR no_jogo LIKE CONCAT('%', NULL, '%'))
  AND (NULL IS NULL OR co_plataforma = NULL);

SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
       st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo
FROM tb_jogo
WHERE (FALSE = TRUE OR st_ativo = TRUE)
  AND (NULL IS NULL OR no_jogo LIKE CONCAT('%', NULL, '%'))
  AND (NULL IS NULL OR co_plataforma = NULL)
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 0;

-- ListarAsync com busca 'Super', página 2. Esperado: total 6, página 2 com 1 jogo
SELECT COUNT(*) FROM tb_jogo
WHERE (FALSE = TRUE OR st_ativo = TRUE)
  AND ('Super' IS NULL OR no_jogo LIKE CONCAT('%', 'Super', '%'))
  AND (NULL IS NULL OR co_plataforma = NULL);

SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE (FALSE = TRUE OR st_ativo = TRUE)
  AND ('Super' IS NULL OR no_jogo LIKE CONCAT('%', 'Super', '%'))
  AND (NULL IS NULL OR co_plataforma = NULL)
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 5;

-- ListarAsync com plataforma 7 (N64) e incluindo inativos. Esperado: 3 jogos
SELECT co_seq_jogo, no_jogo, st_ativo
FROM tb_jogo
WHERE (TRUE = TRUE OR st_ativo = TRUE)
  AND (NULL IS NULL OR no_jogo LIKE CONCAT('%', NULL, '%'))
  AND (7 IS NULL OR co_plataforma = 7)
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 0;

-- ListarAsync com lista de ids (filtro por gênero resolvido no Service).
-- Esperado: Gran Turismo e Super Mario Kart (o 22 é inativo e fica de fora)
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE (FALSE = TRUE OR st_ativo = TRUE)
  AND (NULL IS NULL OR no_jogo LIKE CONCAT('%', NULL, '%'))
  AND (NULL IS NULL OR co_plataforma = NULL)
  AND co_seq_jogo IN (14, 17, 22)
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 0;

-- BuscarPorIdAsync (@id = 19). Esperado: Tetris, com ds_jogo NULL
SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
       st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo
FROM tb_jogo
WHERE co_seq_jogo = 19;

-- InserirAsync, AlterarAsync e DesativarAsync (desfeitos no ROLLBACK)
START TRANSACTION;
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Donkey Kong Country', 'Cartucho original.', 1994, 199.90, 2,
        TRUE, UTC_TIMESTAMP(), 5, 2);
SELECT LAST_INSERT_ID() AS id_gerado;
UPDATE tb_jogo
SET no_jogo = 'Donkey Kong Country', ds_jogo = 'Cartucho revisado.', nu_ano_lancamento = 1994,
    vl_preco = 219.90, qt_estoque = 3, st_ativo = TRUE, co_plataforma = 5, co_condicao_jogo = 3
WHERE co_seq_jogo = LAST_INSERT_ID();
UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = LAST_INSERT_ID();
SELECT co_seq_jogo, no_jogo, vl_preco, st_ativo FROM tb_jogo WHERE co_seq_jogo = LAST_INSERT_ID();
ROLLBACK;

-- =============================================================================
-- Aula 2 - Etapa 4: JogoGeneroRepository
-- =============================================================================

-- ListarTodosAsync. Esperado: 37 vínculos
SELECT co_jogo, co_genero FROM tb_jogo_genero ORDER BY co_jogo, co_genero;

-- ListarPorJogosAsync (@id0 = 1, @id1 = 12). Esperado: 5 vínculos
SELECT co_jogo, co_genero
FROM tb_jogo_genero
WHERE co_jogo IN (1, 12)
ORDER BY co_jogo, co_genero;

-- RemoverPorJogoAsync e InserirAsync (desfeitos no ROLLBACK)
START TRANSACTION;
DELETE FROM tb_jogo_genero WHERE co_jogo = 3;
INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES (3, 3), (3, 2);
SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo = 3;
ROLLBACK;

-- =============================================================================
-- Aula 2 - Etapa 5: GeneroRepository e CondicaoJogoRepository
-- =============================================================================

-- GeneroRepository.ListarAsync. Esperado: 9 gêneros em ordem alfabética
SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero;

-- CondicaoJogoRepository.ListarAsync. Esperado: Novo, Usado, Recondicionado
SELECT co_condicao_jogo, ds_condicao_jogo FROM tb_condicao_jogo ORDER BY co_condicao_jogo;
