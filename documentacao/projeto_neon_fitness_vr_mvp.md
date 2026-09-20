# Projeto Neon Fitness VR — Visão Geral e Especificação do MVP

> Documento de contexto e execução para um agente de desenvolvimento no VS Code.

## 1. Papel do agente

Você é responsável por criar um protótipo funcional de um jogo fitness em realidade virtual para o **Meta Quest 3**, usando **Unity e C#**.

Antes de alterar arquivos:

1. inspecione a estrutura e as versões do projeto existente;
2. identifique os pacotes XR já instalados;
3. elabore um plano curto e incremental;
4. não substitua configurações válidas nem atualize versões sem necessidade;
5. implemente e valide uma etapa por vez.

Se o repositório ainda estiver vazio, crie a estrutura descrita neste documento. Use versões estáveis e mutuamente compatíveis do Unity, OpenXR e dos pacotes da Meta. Registre no `README.md` as versões efetivamente utilizadas.

## 2. Visão do projeto

Criar um jogo VR que combine exercício físico leve, precisão e estética de arena digital neon. A inspiração conceitual vem de jogos como *Aaero*, mas o projeto terá identidade e implementação próprias.

O jogador usa as próprias mãos para interagir com alvos e elementos luminosos. A experiência deve ser confortável, fácil de entender e adequada para sessões curtas.

### Evolução imaginada após o MVP

- trilhas luminosas que o jogador acompanha com as mãos;
- alvos por mão esquerda ou direita;
- sequências, ondas, combos e multiplicadores;
- marcação de alvos com pinça e disparo por gesto;
- diferentes tipos e padrões de alvo;
- efeitos sonoros e música adaptativa;
- fases e modos de jogo;
- placar, histórico e telemetria;
- ambiente central no estilo loft/hub neon;
- eventual backend Java para ranking, perfil e geração de fases.

Esses itens representam a visão futura e **não fazem parte do MVP atual**.

## 3. Objetivo do MVP

Entregar uma experiência standalone executável no Meta Quest 3 em que:

1. o jogador permanece parado no centro de uma arena simples;
2. suas duas mãos são rastreadas sem controles físicos;
3. alvos surgem à frente, deslocam-se em direção ao jogador e podem ser atingidos com um soco ou toque;
4. o alvo desaparece ao ser atingido;
5. o jogo soma pontos e mostra uma interface mínima;
6. a rodada termina após um tempo definido e exibe o resultado.

O objetivo técnico é validar o ciclo completo:

`detectar mãos → criar alvo → mover alvo → detectar impacto → pontuar → encerrar rodada`.

## 4. Plataforma e stack

| Item | Escolha |
|---|---|
| Engine | Unity, versão LTS compatível com o SDK selecionado |
| Linguagem | C# |
| Dispositivo | Meta Quest 3 standalone |
| Runtime XR | OpenXR |
| Integração Quest | Meta XR SDK / Interaction SDK |
| Entrada principal | Hand tracking das duas mãos |
| Renderização | URP ou configuração mobile equivalente já presente no projeto |
| Build | Android/Quest, arquitetura ARM64 |
| Persistência | Nenhuma no MVP |
| Backend | Nenhum no MVP |

Prefira os pacotes e samples oficiais da Meta como referência para configurar o rastreamento das mãos. Não copie uma cena de demonstração inteira para a cena final; reaproveite apenas a configuração e os componentes necessários.

## 5. Escopo funcional do MVP

### 5.1 Arena

- Ambiente pequeno, escuro e abstrato.
- Jogador parado, sem locomoção artificial.
- Piso plano com grade ou linhas neon simples.
- Fundo escuro com poucos elementos geométricos.
- Área de jogo sempre à frente do jogador.
- Não exigir giro de 180°, deslocamento, salto ou agachamento.
- Iluminação e efeitos leves o suficiente para o Quest 3.

Não gastar tempo com modelagem detalhada. Primitivas, materiais emissivos simples e assets livres compatíveis são suficientes.

### 5.2 Mãos

- Exibir ou representar as mãos esquerda e direita rastreadas.
- Cada mão deve possuir uma pequena área de impacto associada à palma ou à região frontal da mão.
- O impacto deve funcionar tanto como toque quanto como movimento semelhante a um soco.
- Não exigir velocidade mínima no MVP; contato válido é suficiente.
- Impedir que um único alvo contabilize mais de um acerto.

### 5.3 Alvos

- Usar um único tipo de alvo no MVP.
- Aparência: esfera, disco ou poliedro com material neon.
- Surgir somente dentro de uma zona confortável à frente do jogador.
- Nascer a uma distância configurável e deslocar-se em linha reta em direção ao jogador.
- Ter velocidade configurável.
- Ser destruído ao colidir com uma das mãos.
- Ser considerado perdido ao ultrapassar o jogador ou um limite definido.
- Nunca nascer perto demais do rosto.

### 5.4 Spawner e rodada

- Gerar um alvo por vez inicialmente.
- Usar intervalo configurável entre alvos.
- Sortear posições dentro de limites seguros de largura e altura.
- Manter todos os valores importantes no Inspector ou em um objeto de configuração.
- Duração sugerida da rodada: 60 segundos, configurável.
- Ao acabar o tempo, parar o spawner e remover ou invalidar alvos ativos.

### 5.5 Pontuação e interface

- Um acerto vale um número configurável de pontos, inicialmente 100.
- Registrar quantidade de acertos e alvos perdidos.
- Mostrar durante a rodada:
  - pontuação;
  - tempo restante;
  - opcionalmente, acertos e erros.
- Mostrar ao final:
  - pontuação total;
  - total de acertos;
  - total de alvos perdidos;
  - botão para reiniciar.
- A UI deve ser legível em VR e estar posicionada à frente sem bloquear os alvos.

### 5.6 Feedback mínimo

- Ao acertar, gerar um flash, partículas leves ou mudança breve de cor.
- Dar feedback visual claro antes de destruir ou desativar o alvo.
- O MVP não terá música.
- Sons também podem ficar de fora; caso seja trivial adicionar um som livre e licenciado, mantenha-o opcional e desligado por padrão.

## 6. Fora do escopo

Não implementar nesta entrega:

- música, sincronização por BPM ou geração musical por IA;
- trilhas curvas no estilo *Aaero*;
- alvos diferentes para cada mão;
- pinça, lock-on, disparos ou reconhecimento de poses especiais;
- combos, multiplicadores, dificuldade adaptativa ou power-ups;
- inimigos, armas, dano ao jogador ou obstáculos;
- agachamentos, saltos ou locomoção;
- passthrough ou realidade mista;
- multiplayer;
- conta, ranking online, telemetria ou backend;
- Java, Spring Boot, AWS ou banco de dados;
- loja, monetização ou processo de publicação;
- substituição do ambiente Home do Quest;
- polimento gráfico avançado.

Não antecipe funcionalidades futuras enquanto os critérios de aceite do MVP não estiverem atendidos.

## 7. Arquitetura sugerida

Evite um único script central. Mantenha responsabilidades pequenas e dependências explícitas.

```text
Assets/
  _Project/
    Art/
      Materials/
      Prefabs/
      VFX/
    Audio/
    Scenes/
      NeonFitnessMVP.unity
    Scripts/
      Core/
        GameManager.cs
        GameState.cs
      Hands/
        HandHitDetector.cs
      Targets/
        Target.cs
        TargetMover.cs
        TargetSpawner.cs
      Scoring/
        ScoreManager.cs
      UI/
        GameHud.cs
        ResultPanel.cs
      Config/
        GameConfig.cs
    Settings/
    Tests/
      EditMode/
      PlayMode/
```

Adapte nomes e pastas à convenção já existente no repositório. Não duplique sistemas que o projeto já possua.

### Responsabilidades

| Componente | Responsabilidade |
|---|---|
| `GameManager` | Controlar estados, tempo da rodada, início, término e reinício |
| `GameState` | Representar estados como `Ready`, `Playing` e `Finished` |
| `HandHitDetector` | Traduzir uma colisão válida da mão em um acerto no alvo |
| `Target` | Garantir ciclo de vida e acerto único; emitir eventos de acerto/perda |
| `TargetMover` | Mover o alvo e detectar que passou do limite |
| `TargetSpawner` | Criar alvos em posições seguras durante a rodada |
| `ScoreManager` | Calcular e armazenar pontuação, acertos e perdas |
| `GameHud` | Atualizar pontuação e cronômetro |
| `ResultPanel` | Exibir resultado e solicitar reinício |
| `GameConfig` | Concentrar valores ajustáveis, preferencialmente em `ScriptableObject` |

Use eventos C# ou referências serializadas para comunicação simples. Evite service locator, framework de injeção de dependência ou arquitetura excessiva para este MVP.

## 8. Parâmetros iniciais sugeridos

Todos devem ser ajustáveis sem alteração de código.

| Parâmetro | Valor inicial sugerido |
|---|---:|
| Duração da rodada | 60 s |
| Intervalo entre alvos | 1,5 s |
| Pontos por acerto | 100 |
| Velocidade do alvo | 1,5 m/s |
| Distância inicial | 3,0 m |
| Altura mínima | 1,0 m |
| Altura máxima | 1,8 m |
| Deslocamento horizontal máximo | 0,75 m para cada lado |
| Raio aproximado do alvo | 0,15 m |

Esses valores são apenas ponto de partida. Priorize conforto e alcance natural dos braços.

## 9. Requisitos técnicos e de qualidade

- Compilar sem erros nem avisos criados pelo código do projeto.
- Evitar alocações por frame nos scripts principais.
- Não usar `FindObjectOfType`, busca por tag ou reflexão continuamente em `Update`.
- Manter referências em cache.
- Usar `Time.deltaTime` no movimento.
- Evitar física complexa; colliders simples são suficientes.
- Preferir pool de objetos se a criação/destruição causar picos perceptíveis. Para a primeira prova, instanciação simples é aceitável; registre a decisão.
- Evitar pós-processamento pesado, transparências extensas e excesso de partículas.
- Não incluir assets sem licença clara para reutilização.
- Não colocar segredos, credenciais ou identificadores pessoais no projeto.
- Documentar configuração manual inevitável no `README.md`.

## 10. Segurança e conforto

- Exibir uma tela inicial breve orientando o jogador a liberar espaço ao redor.
- Manter alvos no campo frontal e em alcance confortável.
- Não gerar alvos atrás do jogador, junto ao chão ou acima da cabeça.
- Não incentivar socos com força; o jogo deve reconhecer contato ou movimento moderado.
- Não posicionar alvos tão próximos que a mão possa atingir o headset.
- Respeitar o guardian/boundary do Quest; não tentar ocultá-lo ou contorná-lo.
- Evitar flashes intensos e movimentos de câmera artificiais.
- A câmera deve acompanhar apenas a cabeça, sem balanço ou deslocamento imposto pelo jogo.

## 11. Fluxo do jogo

```text
Abrir app
  → carregar arena e rastreamento das mãos
  → mostrar orientação e botão Iniciar
  → iniciar cronômetro e spawner
  → alvo surge à frente
  → alvo avança
  → mão encosta: acerto, feedback e pontos
  → alvo passa do limite: registrar perda
  → repetir até o tempo terminar
  → interromper geração
  → mostrar resultado
  → permitir reiniciar a rodada
```

## 12. Plano de implementação

Execute na ordem abaixo e mantenha o projeto utilizável após cada etapa:

### Etapa 1 — Fundação XR

- Configurar Android, OpenXR e os pacotes da Meta.
- Criar a cena principal e o rig XR.
- Confirmar rastreamento das duas mãos no Quest 3.
- Registrar instruções de build e execução.

### Etapa 2 — Protótipo de impacto

- Criar um alvo fixo.
- Adicionar colisores adequados às mãos e ao alvo.
- Detectar uma colisão válida uma única vez.
- Exibir feedback visual e remover o alvo.

### Etapa 3 — Movimento e geração

- Criar `TargetMover` e limite de alvo perdido.
- Criar `TargetSpawner` com área frontal configurável.
- Garantir que alvos não nasçam perto do rosto nem fora da área segura.

### Etapa 4 — Loop de rodada

- Implementar estados do jogo.
- Adicionar cronômetro, pontuação, acertos e perdas.
- Interromper corretamente a rodada e permitir reinício limpo.

### Etapa 5 — Apresentação mínima

- Criar arena escura simples com grid neon.
- Adicionar HUD e painel de resultado legíveis.
- Adicionar feedback visual leve no acerto.

### Etapa 6 — Validação

- Testar em Play Mode o que for possível.
- Fazer build Android ARM64.
- Testar no Quest 3 com hand tracking real.
- Corrigir colisões duplicadas, alvos inalcançáveis e problemas de reinício.
- Registrar limitações conhecidas.

## 13. Estratégia de testes

Crie testes automáticos somente onde tragam valor sem depender do hardware:

- `ScoreManager` soma pontos apenas uma vez por alvo;
- contadores de acerto e perda permanecem consistentes;
- transições de estado seguem `Ready → Playing → Finished`;
- fim da rodada impede novos spawns;
- reinício zera tempo, pontuação e contadores;
- posições produzidas pelo spawner permanecem dentro dos limites configurados.

Validações obrigatórias no headset:

- ambas as mãos são reconhecidas;
- o contato acerta o alvo de modo confiável;
- não há dupla pontuação;
- todos os alvos são alcançáveis sem agachar ou girar;
- interface é legível;
- reiniciar não deixa alvos antigos na cena;
- desempenho permanece confortável e estável.

## 14. Critérios de aceite

O MVP está concluído apenas quando:

- [ ] uma build standalone instala e abre no Meta Quest 3;
- [ ] o jogo funciona com hand tracking, sem controles;
- [ ] as duas mãos conseguem atingir os alvos;
- [ ] alvos aparecem apenas à frente e avançam em direção ao jogador;
- [ ] cada alvo contabiliza no máximo um acerto;
- [ ] alvos não atingidos são registrados como perdidos;
- [ ] pontuação e tempo restante aparecem durante a rodada;
- [ ] a rodada termina automaticamente;
- [ ] a tela final mostra os resultados;
- [ ] é possível reiniciar sem recarregar o aplicativo;
- [ ] o ambiente possui visual neon simples e adequado ao Quest 3;
- [ ] não há música, backend ou funcionalidades futuras misturadas ao MVP;
- [ ] o `README.md` explica instalação, versões, build, execução e limitações.

## 15. Entregáveis esperados

1. Projeto Unity organizado e versionável.
2. Cena `NeonFitnessMVP` funcional.
3. Scripts C# com responsabilidades separadas.
4. Prefab do alvo e configuração dos colliders das mãos.
5. Materiais e VFX neon mínimos.
6. Testes automatizados relevantes.
7. `README.md` com:
   - pré-requisitos e versões;
   - pacotes necessários;
   - configuração do Quest em modo desenvolvedor;
   - como abrir, testar e gerar a build;
   - como instalar/executar no headset;
   - parâmetros ajustáveis;
   - limitações conhecidas.

## 16. Regras para o agente durante a execução

- Não invente APIs ou componentes: confira o código e a documentação correspondente à versão instalada.
- Não misture simultaneamente configurações concorrentes de XR sem justificativa.
- Não faça atualizações amplas de pacotes para resolver um problema localizado.
- Não edite arquivos gerados ou diretórios como `Library`, `Temp`, `Logs` e `Build`.
- Preserve mudanças existentes do usuário.
- Para cada etapa, informe arquivos alterados, como validar e qualquer configuração manual necessária no Unity Editor.
- Quando uma ação depender do headset ou do Editor e não puder ser validada automaticamente, forneça passos exatos de verificação e aguarde o resultado antes de assumir sucesso.
- Priorize primeiro o ciclo jogável; somente depois faça o polimento neon.

## 17. Primeira tarefa do agente

Comece da seguinte forma:

1. examine o repositório e informe se já existe um projeto Unity válido;
2. identifique a versão do Unity e os pacotes XR instalados;
3. compare o estado atual com esta especificação;
4. proponha um plano curto para concluir apenas a **Etapa 1 — Fundação XR**;
5. só então implemente a etapa, sem avançar automaticamente para as seguintes;
6. ao terminar, informe como validar o rastreamento das duas mãos no Meta Quest 3.

