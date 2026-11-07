-- =============================================================================
-- RetroGames - Script 03: consultas do catálogo
-- Referência: Espec. 4.2 e 4.3; Fundamentos, tema 4
--
-- Estas consultas usam JOIN porque são para estudo e conferência no Workbench.
-- Na API, a regra é outra: cada consulta do Repository lê uma única tabela e a
-- combinação dos dados é feita no Service, com LINQ (Espec. 3.1).
--
-- Rode uma consulta por vez (cursor na consulta + Ctrl+Enter).
-- =============================================================================

USE retrogames_beatriz;

-- -----------------------------------------------------------------------------
-- 1. Catálogo: jogos ativos com o nome da plataforma e a descrição da condição,
--    ordenados por nome.
--    Esperado: 21 jogos (o Mario Kart 64, inativo, não aparece).
-- -----------------------------------------------------------------------------
SELECT j.co_seq_jogo,
       j.no_jogo,
       p.no_plataforma,
       c.ds_condicao_jogo,
       j.nu_ano_lancamento,
       j.vl_preco,
       j.qt_estoque
FROM tb_jogo j
INNER JOIN tb_plataforma    p ON p.co_seq_plataforma = j.co_plataforma
INNER JOIN tb_condicao_jogo c ON c.co_condicao_jogo  = j.co_condicao_jogo
WHERE j.st_ativo = TRUE
ORDER BY j.no_jogo;

-- -----------------------------------------------------------------------------
-- 2. Gêneros de um jogo informado.
--    Troque o valor de @id_jogo para consultar outro jogo.
--    Esperado com 12 (Super Metroid): Ação, Aventura, Tiro.
-- -----------------------------------------------------------------------------
SET @id_jogo = 12;

SELECT g.co_seq_genero,
       g.no_genero
FROM tb_jogo_genero jg
INNER JOIN tb_genero g ON g.co_seq_genero = jg.co_genero
WHERE jg.co_jogo = @id_jogo
ORDER BY g.no_genero;

-- -----------------------------------------------------------------------------
-- 3. Jogos de um gênero informado.
--    Troque o valor de @id_genero para consultar outro gênero.
--    Esperado com 5 (Corrida): Gran Turismo, Mario Kart 64, Super Mario Kart.
-- -----------------------------------------------------------------------------
SET @id_genero = 5;

SELECT j.co_seq_jogo,
       j.no_jogo,
       j.st_ativo
FROM tb_jogo_genero jg
INNER JOIN tb_jogo j ON j.co_seq_jogo = jg.co_jogo
WHERE jg.co_genero = @id_genero
ORDER BY j.no_jogo;

-- -----------------------------------------------------------------------------
-- 4. Quantidade de jogos por plataforma, incluindo as plataformas sem jogos.
--    O LEFT JOIN mantém a plataforma mesmo sem jogo, e COUNT(j.co_seq_jogo)
--    conta só as linhas com jogo (COUNT(*) contaria 1 para a plataforma vazia).
--    Esperado: Neo Geo AES com 0.
-- -----------------------------------------------------------------------------
SELECT p.co_seq_plataforma,
       p.no_plataforma,
       COUNT(j.co_seq_jogo) AS qt_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON j.co_plataforma = p.co_seq_plataforma
GROUP BY p.co_seq_plataforma, p.no_plataforma
ORDER BY p.no_plataforma;

-- -----------------------------------------------------------------------------
-- 5. Busca por parte do nome do jogo (jogos ativos), com 5 itens por página.
--    OFFSET = (página - 1) * 5.
--    Esperado com 'Super': 6 resultados -> 5 na página 1 e 1 na página 2.
-- -----------------------------------------------------------------------------
SET @busca = 'Super';

-- Total de resultados (para calcular o número de páginas)
SELECT COUNT(*) AS total
FROM tb_jogo
WHERE st_ativo = TRUE
  AND no_jogo LIKE CONCAT('%', @busca, '%');

-- Página 1
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE st_ativo = TRUE
  AND no_jogo LIKE CONCAT('%', @busca, '%')
ORDER BY no_jogo
LIMIT 5 OFFSET 0;

-- Página 2
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE st_ativo = TRUE
  AND no_jogo LIKE CONCAT('%', @busca, '%')
ORDER BY no_jogo
LIMIT 5 OFFSET 5;
