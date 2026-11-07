-- =============================================================================
-- RetroGames - Script 02: dados de exemplo
-- Rode logo depois do 01_criar_tabelas.sql, num banco recém-criado.
--
-- Os ids são informados explicitamente para deixar claras as associações em
-- tb_jogo_genero. Não há usuários: o administrador é criado pela API
-- (aula de autenticação).
-- =============================================================================

USE retrogames_beatriz;

-- -----------------------------------------------------------------------------
-- Domínios com chave fixa (mesmos valores de CondicaoJogoIds e PerfilIds)
-- -----------------------------------------------------------------------------

INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
    (1, 'Novo'),
    (2, 'Usado'),
    (3, 'Recondicionado');

INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
    (1, 'Administrador'),
    (2, 'Cliente');

-- -----------------------------------------------------------------------------
-- Plataformas (8). A Neo Geo AES fica sem jogos de propósito, para testar a
-- contagem por plataforma com resultado 0 (Etapa 5).
-- -----------------------------------------------------------------------------

INSERT INTO tb_plataforma (co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
    (1, 'Atari 2600',     'Atari',    1977),
    (2, 'NES',            'Nintendo', 1983),
    (3, 'Master System',  'Sega',     1985),
    (4, 'Mega Drive',     'Sega',     1988),
    (5, 'Super Nintendo', 'Nintendo', 1990),
    (6, 'PlayStation',    'Sony',     1994),
    (7, 'Nintendo 64',    'Nintendo', 1996),
    (8, 'Neo Geo AES',    'SNK',      1990);

-- -----------------------------------------------------------------------------
-- Gêneros (9)
-- -----------------------------------------------------------------------------

INSERT INTO tb_genero (co_seq_genero, no_genero) VALUES
    (1, 'Ação'),
    (2, 'Aventura'),
    (3, 'Plataforma'),
    (4, 'RPG'),
    (5, 'Corrida'),
    (6, 'Luta'),
    (7, 'Esporte'),
    (8, 'Tiro'),
    (9, 'Puzzle');

-- -----------------------------------------------------------------------------
-- Jogos (22). Casos especiais:
--   jogo  5 (Mega Man 2)    -> estoque zero
--   jogo 22 (Mario Kart 64) -> inativo (st_ativo = FALSE)
--   jogo 19 (Tetris)        -> sem descrição (ds_jogo NULL)
-- Condição: 1 Novo, 2 Usado, 3 Recondicionado. dt_cadastro em UTC.
-- -----------------------------------------------------------------------------

INSERT INTO tb_jogo (co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) VALUES
    ( 1, 'Pitfall!',                               'Cartucho original, sem caixa.',                1982,  89.90, 3, TRUE,  UTC_TIMESTAMP(), 1, 2),
    ( 2, 'River Raid',                             'Cartucho original com etiqueta conservada.',   1982,  79.90, 2, TRUE,  UTC_TIMESTAMP(), 1, 2),
    ( 3, 'Super Mario Bros.',                      'Cartucho original, testado.',                  1985, 249.90, 4, TRUE,  UTC_TIMESTAMP(), 2, 2),
    ( 4, 'The Legend of Zelda',                    'Cartucho dourado com bateria nova.',           1986, 299.90, 2, TRUE,  UTC_TIMESTAMP(), 2, 3),
    ( 5, 'Mega Man 2',                             'Cartucho com caixa e manual.',                 1988, 219.90, 0, TRUE,  UTC_TIMESTAMP(), 2, 2),
    ( 6, 'Alex Kidd in Miracle World',             'Cartucho original, sem caixa.',                1986, 129.90, 5, TRUE,  UTC_TIMESTAMP(), 3, 2),
    ( 7, 'Phantasy Star',                          'Cartucho com bateria de save substituída.',   1987, 349.90, 1, TRUE,  UTC_TIMESTAMP(), 3, 3),
    ( 8, 'Sonic the Hedgehog',                     'Cartucho com caixa.',                          1991, 159.90, 6, TRUE,  UTC_TIMESTAMP(), 4, 2),
    ( 9, 'Streets of Rage 2',                      'Cartucho com caixa e manual.',                 1992, 199.90, 3, TRUE,  UTC_TIMESTAMP(), 4, 2),
    (10, 'Mortal Kombat',                          'Cartucho original, sem manual.',               1993, 149.90, 4, TRUE,  UTC_TIMESTAMP(), 4, 2),
    (11, 'Super Mario World',                      'Cartucho original, testado.',                  1990, 279.90, 5, TRUE,  UTC_TIMESTAMP(), 5, 2),
    (12, 'Super Metroid',                          'Cartucho com contatos limpos e revisado.',     1994, 389.90, 2, TRUE,  UTC_TIMESTAMP(), 5, 3),
    (13, 'Chrono Trigger',                         'Cartucho com bateria de save nova.',           1995, 699.90, 1, TRUE,  UTC_TIMESTAMP(), 5, 3),
    (14, 'Super Mario Kart',                       'Lacrado, caixa original de fábrica.',          1992, 899.90, 1, TRUE,  UTC_TIMESTAMP(), 5, 1),
    (15, 'International Superstar Soccer Deluxe',  'Cartucho original, sem caixa.',                1995, 119.90, 4, TRUE,  UTC_TIMESTAMP(), 5, 2),
    (16, 'Final Fantasy VII',                      'Quatro discos, caixa e manual.',               1997, 199.90, 3, TRUE,  UTC_TIMESTAMP(), 6, 2),
    (17, 'Gran Turismo',                           'Disco sem riscos, com manual.',                1997,  99.90, 5, TRUE,  UTC_TIMESTAMP(), 6, 2),
    (18, 'Crash Bandicoot',                        'Disco com caixa.',                             1996, 129.90, 4, TRUE,  UTC_TIMESTAMP(), 6, 2),
    (19, 'Tetris',                                 NULL,                                           1989,  89.90, 6, TRUE,  UTC_TIMESTAMP(), 2, 2),
    (20, 'Super Mario 64',                         'Cartucho original, testado.',                  1996, 349.90, 3, TRUE,  UTC_TIMESTAMP(), 7, 2),
    (21, 'GoldenEye 007',                          'Cartucho original, sem caixa.',                1997, 229.90, 2, TRUE,  UTC_TIMESTAMP(), 7, 2),
    (22, 'Mario Kart 64',                          'Fora de venda: aguardando revisão.',           1996, 279.90, 1, FALSE, UTC_TIMESTAMP(), 7, 2);

-- -----------------------------------------------------------------------------
-- Gêneros de cada jogo (1 a 3 por jogo)
-- -----------------------------------------------------------------------------

INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
    ( 1, 2), ( 1, 3),          -- Pitfall!: Aventura, Plataforma
    ( 2, 8), ( 2, 1),          -- River Raid: Tiro, Ação
    ( 3, 3),                   -- Super Mario Bros.: Plataforma
    ( 4, 2), ( 4, 1),          -- The Legend of Zelda: Aventura, Ação
    ( 5, 1), ( 5, 3),          -- Mega Man 2: Ação, Plataforma
    ( 6, 3), ( 6, 2),          -- Alex Kidd: Plataforma, Aventura
    ( 7, 4),                   -- Phantasy Star: RPG
    ( 8, 3), ( 8, 1),          -- Sonic: Plataforma, Ação
    ( 9, 1), ( 9, 6),          -- Streets of Rage 2: Ação, Luta
    (10, 6),                   -- Mortal Kombat: Luta
    (11, 3),                   -- Super Mario World: Plataforma
    (12, 1), (12, 2), (12, 8), -- Super Metroid: Ação, Aventura, Tiro
    (13, 4), (13, 2),          -- Chrono Trigger: RPG, Aventura
    (14, 5),                   -- Super Mario Kart: Corrida
    (15, 7),                   -- ISS Deluxe: Esporte
    (16, 4), (16, 2),          -- Final Fantasy VII: RPG, Aventura
    (17, 5), (17, 7),          -- Gran Turismo: Corrida, Esporte
    (18, 3), (18, 1),          -- Crash Bandicoot: Plataforma, Ação
    (19, 9),                   -- Tetris: Puzzle
    (20, 3), (20, 2),          -- Super Mario 64: Plataforma, Aventura
    (21, 8), (21, 1),          -- GoldenEye 007: Tiro, Ação
    (22, 5);                   -- Mario Kart 64: Corrida
