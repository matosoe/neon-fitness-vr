# Plano de implementação — Neon Fitness VR com agentes do Codex no VS Code

> Plano vivo para implementar o MVP descrito em [`projeto_neon_fitness_vr_mvp.md`](../../projeto_neon_fitness_vr_mvp.md).
>
> **Atualizado em:** 2026-09-19  
> **Plataforma-alvo:** Meta Quest 3 standalone  
> **Stack:** Unity 6, C#, URP, OpenXR, Meta XR Core SDK e Meta XR Interaction SDK  
> **Estado geral:** 🟡 preparação

## 1. Como usar e atualizar este plano

Este arquivo é a fonte de verdade operacional do projeto. O agente principal deve lê-lo junto com a especificação do MVP antes de iniciar qualquer etapa.

### Legenda

- `[ ]` não iniciado;
- `[x]` concluído e verificado;
- `🔄` em andamento — escrever ao lado da tarefa o agente e a data;
- `⛔` bloqueado — registrar o motivo na seção **Bloqueios e decisões pendentes**;
- `👤` exige ação ou validação humana;
- `🤖` pode ser executado pelo Codex;
- `🥽` exige o Quest 3 físico;
- `GATE` ponto de parada: não avançar sem evidência do critério de saída.

### Regra de atualização por agente

Ao assumir uma tarefa, o agente deve:

1. confirmar que todas as dependências da tarefa estão concluídas;
2. acrescentar `🔄 — agente: <nome>; início: AAAA-MM-DD` na linha da tarefa;
3. fazer apenas as alterações daquela tarefa;
4. executar as validações descritas;
5. substituir por `[x]` somente quando houver evidência verificável;
6. registrar no **Diário de execução** os arquivos alterados, comandos/testes, resultado e próximo passo;
7. se depender do Editor, conta ou headset, marcar `⛔` e fornecer ao usuário um roteiro exato — nunca presumir sucesso;
8. encerrar com o repositório compilável e sem misturar fases futuras.

Formato recomendado para cada registro:

```markdown
### AAAA-MM-DD — ID da tarefa — agente

- Resultado: concluído | parcial | bloqueado
- Arquivos alterados: `caminho`
- Validação: comando ou procedimento + resultado
- Evidência: log, captura, APK ou observação humana
- Decisões: link para ADR, se houver
- Próximo passo: ID da tarefa
```

## 2. Estado inicial observado

### Repositório

- [x] Há um repositório Git em `C:\projetos\neon-fitness-vr`.
- [x] A especificação do MVP existe em `projeto_neon_fitness_vr_mvp.md`.
- [x] Ainda não há um projeto Unity: faltam `Assets/`, `Packages/` e `ProjectSettings/`.
- [x] O repositório ainda não possui commits; a especificação está como arquivo não rastreado.
- [x] A pasta deste plano foi criada como `documentacao/planos/` (nome sem acentos para maior portabilidade entre ferramentas).

### Ferramentas detectadas nesta máquina

| Ferramenta | Estado em 2026-09-19 | Ação |
|---|---|---|
| Git | ✅ 2.49.0 | manter |
| Git LFS | ✅ 3.6.1 | inicializar no repositório e criar regras |
| VS Code | ✅ 1.138.0 | manter |
| Extensão Codex (`openai.chatgpt`) | ✅ instalada | confirmar login e acesso ao workspace |
| Codex CLI embarcado | ✅ detectado | opcional; extensão é a interface principal |
| Extensão Unity da Microsoft | ❌ não detectada | instalar |
| Unity Hub / Unity Editor | ⚠️ não detectados no `PATH` nem no local padrão | instalar ou localizar instalação existente |
| ADB | ⚠️ não detectado no `PATH` | virá com o módulo Android do Unity; configurar depois |
| Meta Quest Developer Hub (MQDH) | ❓ não verificado | instalar |
| Meta Horizon Link | ❓ não verificado | instalar se for usar Play Mode no headset |

> `⚠️ não detectado` não prova ausência total: antes de instalar, o responsável deve conferir **Aplicativos instalados** e instalações em caminhos personalizados.

## 3. Decisões técnicas de base

Estas são as decisões propostas. Qualquer alteração deve ser registrada em `documentacao/decisoes/ADR-NNN-titulo.md`.

| Tema | Decisão para o MVP | Motivo |
|---|---|---|
| Editor | Unity **6.3 LTS**, patch estável mais recente oferecido pelo Unity Hub/MQDH e aprovado pela validação da Meta | é a LTS atual, com suporte informado até dezembro de 2027; fixar o patch efetivo no projeto |
| Template | Universal 3D / URP | caminho atual recomendado pela Meta e adequado ao visual neon mobile |
| Runtime | Unity OpenXR Plugin | recomendado para projetos novos; Oculus XR Plugin está depreciado |
| Integração Quest | Meta XR Core SDK + Meta XR Interaction SDK | fornece rig, hand tracking e interações; evita SDKs fora do escopo |
| Pacote Meta | instalar apenas Core + Interaction e dependências | o All-in-One é útil para protótipo, mas inclui Platform, Voice, MRUK e outros módulos desnecessários |
| Mãos | Interaction SDK, mãos apenas; esqueleto OpenXR; colliders/cápsulas de física | recomendação atual da Meta e compatibilidade futura melhor |
| Renderização | URP, forward, mobile, sem pós-processamento pesado | desempenho e simplicidade no Quest 3 |
| Build | Meta Quest/Android, IL2CPP, ARM64, APK de desenvolvimento | execução standalone no headset |
| Persistência/backend | nenhum | fora do escopo do MVP |
| App ID da Meta | não criar na fase local | Platform SDK/App ID só é necessário para recursos de plataforma e distribuição; não é requisito para o loop local deste MVP |
| Paralelismo de agentes | um escritor por vez; subagentes paralelos apenas para pesquisa, inspeção, testes independentes e revisão | cenas/prefabs/arquivos `.meta` do Unity geram conflitos com facilidade |

### Compatibilidade a confirmar antes de gerar o projeto

- [ ] 👤/🤖 Confirmar no MQDH e nas notas atuais da Meta que o patch escolhido do Unity 6.3 LTS está recomendado/aceito.
- [ ] 🤖 Registrar a versão exata do Editor em `ProjectSettings/ProjectVersion.txt` e no `README.md`.
- [ ] 🤖 Registrar versões resolvidas de OpenXR, Core SDK e Interaction SDK em `Packages/manifest.json`, `Packages/packages-lock.json` e no `README.md`.
- [ ] 🤖 Executar o Meta Project Setup Tool e zerar erros obrigatórios; não aplicar correções às cegas.
- [ ] GATE: nenhuma API será codificada antes de os pacotes efetivamente instalados e suas APIs serem inspecionados.

## 4. Cadastros e acessos necessários

### 4.1 Obrigatórios para desenvolver e testar o MVP

- [x] 👤 **Conta OpenAI/ChatGPT com acesso ao Codex**
  - Entrar na extensão oficial do Codex no VS Code.
  - Abrir este repositório como workspace confiável.
  - Não é necessária uma chave da API OpenAI para este jogo; o MVP não chama APIs da OpenAI.
  - Evidência: painel do Codex abre e consegue resumir a especificação do projeto.

- [ ] 👤 **Unity ID e organização Unity**
  - Criar/usar um Unity ID e aceitar os termos aplicáveis.
  - Criar ou selecionar uma organização no Unity Hub.
  - Ativar a licença adequada. Unity Personal é elegível para indivíduos/organizações abaixo do limite publicado de US$ 200 mil de receita + captação nos 12 meses anteriores; confirmar a elegibilidade no momento da ativação.
  - Não registrar senha, token ou arquivo de licença no Git.
  - Evidência: Unity Hub autenticado e Editor abre sem erro de licença.

- [ ] 👤 **Conta Meta e cadastro Meta Horizon Developer**
  - Usar a mesma conta no portal de desenvolvedor, no app Meta Horizon, no Quest e no MQDH.
  - Ter 18 anos ou mais para habilitar o modo desenvolvedor.
  - Criar ou entrar em um **developer team/organização**.
  - Verificar a conta e a organização. Para pessoa física/estúdio pequeno, a Meta informa que a verificação do administrador com documento oficial costuma ser o caminho mais simples; empresa pode usar verificação empresarial.
  - Ativar autenticação de dois fatores e guardar códigos de recuperação fora do repositório.
  - Evidência: Developer Dashboard acessível e organização/time aparece como verificado.

- [ ] 👤 **Meta Horizon mobile app**
  - Instalar em Android/iOS, entrar com a mesma conta e parear o Quest 3.
  - Ativar `Developer Mode` nas configurações do headset pelo aplicativo.
  - Evidência: opção de desenvolvedor ativa e aviso de depuração USB aparece no headset.

### 4.2 Obrigatórios somente para publicação — adiar até depois do MVP

- [ ] 👤 Criar um app no Meta Horizon Developer Dashboard e obter App ID.
- [ ] 👤 Definir nome, package/application ID definitivo, política de privacidade, dados coletados, classificação etária, imagens e informações da loja.
- [ ] 👤 Configurar assinatura/keystore de release e procedimento seguro de backup.
- [ ] 👤 Completar Data Use Checkup e demais declarações exigidas para os recursos realmente usados.
- [ ] 👤 Criar canais/release tracks e adicionar testadores, se necessário.

> **Importante:** cadastro de app, Platform SDK e App ID não são necessários para instalar um APK local por ADB e validar o MVP. Não introduzi-los antecipadamente.

### 4.3 Opcionais

- [ ] 👤 Conta GitHub/GitLab/Azure DevOps e repositório remoto privado para backup/colaboração.
- [ ] 👤 Registro de marca/domínio, empresa formal e conta bancária — somente se houver publicação/comercialização.
- [ ] 👤 Ferramentas de arte/áudio — somente após o loop jogável; conferir licenças de cada asset.

## 5. Ferramentas a instalar

### 5.1 Obrigatórias

- [x] Git para Windows.
- [x] Git LFS.
- [x] Visual Studio Code.
- [x] Extensão oficial **Codex** (`openai.chatgpt`) no VS Code.
- [x] 👤 Unity Hub 3.21.3.
- [x] 👤 Unity 6.3 LTS — `6000.3.24f1`, patch LTS mais recente listado pelo Unity CLI em 2026-09-19 e compatível com os requisitos atuais publicados pela Meta.
- [x] 👤 Módulos do Editor instalados pelo Unity CLI:
  - [x] Android Build Support;
  - [x] Android SDK & NDK Tools;
  - [x] OpenJDK.
- [x] 👤 Extensão **Unity** da Microsoft no VS Code (`visualstudiotoolsforunity.vstuc@1.3.1`), com C# Dev Kit e C# instalados.
- [x] 👤 Meta Quest Developer Hub (MQDH) `6.4.1`.
- [x] 👤 Driver USB da Meta/Oculus para ADB no Windows (`12.0.0.0`, pacote `oem110.inf`).
- [ ] 👤 Cabo USB-C com dados, não apenas carga.
- [ ] 🥽 Meta Quest 3 atualizado e pareado.

### 5.2 Recomendadas ou opcionais

- [ ] 👤 Meta Horizon Link para testar hand tracking no Play Mode no Windows e acelerar iterações.
- [ ] 🤖 Meta XR Simulator, instalado como parte do conjunto Meta quando útil; ele não substitui a validação no headset.
- [ ] 🤖 Unity Test Framework, via Package Manager, para EditMode/PlayMode.
- [ ] 👤 Ferramenta de merge YAML do Unity (`UnityYAMLMerge`) configurada no Git caso mais de uma pessoa edite cenas/prefabs.
- [ ] 👤 Android Studio **não é necessário** para este fluxo Unity; instalar apenas se surgir uma necessidade Android específica não coberta pelo SDK/NDK/JDK do Unity.
- [ ] 👤 Meta XR Operator **não é necessário** e é experimental; não adicioná-lo ao caminho crítico do MVP.

### 5.3 Verificação da estação

Executar no PowerShell depois das instalações:

```powershell
git --version
git lfs version
code --version
code --list-extensions | Select-String 'openai.chatgpt|visualstudiotoolsforunity'
adb version
adb devices
```

Também verificar:

- [ ] Unity Hub mostra o Editor e os três módulos Android.
- [ ] VS Code aparece em `Unity > Edit > Preferences > External Tools`.
- [ ] O pacote Unity `Visual Studio Editor` está em versão compatível com a extensão (a documentação do VS Code exige 2.0.20 ou superior).
- [ ] `adb devices` lista o Quest como `device`, não `unauthorized`.
- [ ] MQDH mostra o headset como `Active`.
- [ ] GATE: um APK vazio abre no headset antes de implementar gameplay.

## 6. Preparação do repositório para Unity e agentes

### P0.1 — Baseline do Git

- [x] 🤖 Preservar a especificação original sem reescrevê-la.
- [x] 🤖 Adicionar `.gitignore` oficial/adequado a Unity, incluindo no mínimo `Library/`, `Temp/`, `Logs/`, `obj/`, `Build/`, `Builds/`, `.vs/` e caches locais.
- [x] 🤖 Executar `git lfs install`.
- [x] 🤖 Criar `.gitattributes` e rastrear em LFS apenas binários grandes que realmente entrarem no projeto, por exemplo `*.psd`, `*.blend`, `*.fbx`, `*.wav`, `*.mp3`, `*.mp4`; não colocar cenas, prefabs, `.meta` ou scripts no LFS.
- [x] 🤖 Configurar no Unity `Asset Serialization = Force Text` e `Version Control = Visible Meta Files`.
- [ ] 👤 Criar o primeiro commit/checkpoint depois de revisar os arquivos.
- [ ] Critério de saída: `git status` limpo e clone/reabertura preservam todos os assets e respectivos `.meta`.

### P0.2 — Instruções para o Codex

- [x] 🤖 Criar `AGENTS.md` na raiz com:
  - comandos de validação disponíveis;
  - versão fixa do Unity e stack XR;
  - regra de nunca editar `Library/`, `Temp/`, `Logs/`, builds ou `PackageCache`;
  - regra de preservar `.meta` e não mover assets sem seus `.meta`;
  - arquitetura e namespaces;
  - requisito de não avançar por gates de hardware sem confirmação humana;
  - requisito de um único agente escritor para cenas/prefabs/configurações Unity;
  - seção `## Code Review Rules` com riscos de XR, desempenho, conforto e dupla pontuação.
- [x] 🤖 Criar `.codex/config.toml` apenas se for necessário ajustar agentes por projeto; dispensado nesta etapa por não haver ajuste necessário.
- [x] 🤖 Opcionalmente criar agentes customizados em `.codex/agents/`; dispensados nesta etapa para manter a configuração mínima:
  - `unity_explorer.toml`: somente leitura, mapeia pacotes/cenas/APIs;
  - `gameplay_worker.toml`: implementa uma tarefa C# bem delimitada;
  - `xr_reviewer.toml`: revisa XR, conforto, performance e compatibilidade;
  - `test_reviewer.toml`: procura lacunas e executa testes sem editar cenas.
- [ ] 🤖 Confirmar em uma nova conversa do Codex quais instruções foram carregadas.
- [ ] Critério de saída: o Codex consegue informar stack, comandos e restrições sem reler toda a conversa.

### P0.3 — Estrutura de acompanhamento

- [x] 🤖 Criar `documentacao/decisoes/` e template curto de ADR.
- [x] 🤖 Criar `documentacao/testes/checklist-headset.md`.
- [x] 🤖 Criar `documentacao/testes/matriz-rastreabilidade.md`, ligando requisitos do MVP a teste/evidência.
- [x] 🤖 Criar `README.md` inicial com estado, pré-requisitos e link para este plano.
- [x] Critério de saída: toda tarefa futura tem local para evidência e toda decisão de stack é rastreável.

## 7. Modelo de execução por agentes no Codex/VS Code

### 7.1 Papéis

| Papel | Responsabilidade | Pode escrever? |
|---|---|---|
| Agente principal/orquestrador | seleciona a próxima tarefa, protege escopo, integra, valida e atualiza este plano | sim |
| `unity_explorer` | inspeciona versões, packages, cenas, prefabs e APIs reais | não |
| `gameplay_worker` | altera scripts/testes de uma tarefa com arquivos exclusivos | sim, quando for o único escritor |
| `xr_reviewer` | revisa rig, hand tracking, conforto, manifest e performance | não por padrão |
| `test_reviewer` | executa testes, lê logs e relata falhas | não por padrão |
| Usuário/testador | realiza login, ações no Editor, autoriza USB e valida no headset | ações manuais |

### 7.2 Regras de paralelismo

- Até três subagentes podem trabalhar em paralelo em tarefas de leitura/revisão independentes.
- Nunca permitir dois agentes editando simultaneamente a mesma cena, prefab, `ProjectSettings`, `manifest.json`, `packages-lock.json` ou seus `.meta`.
- Somente o agente principal consolida resultados e marca o gate como concluído.
- Não usar subagente para uma tarefa pequena e sequencial; o custo e a coordenação não compensam.
- Após qualquer alteração de cena/prefab pelo Editor, salvar, fechar processos de build e revisar o diff YAML antes de outra escrita.
- Criar checkpoint Git no fim de cada fase aprovada, não no meio de um estado quebrado.

### 7.3 Prompt padrão para iniciar uma tarefa

```text
Leia AGENTS.md, projeto_neon_fitness_vr_mvp.md e
documentacao/planos/plano-implementacao-codex-vscode.md.
Execute somente a tarefa <ID>. Use subagentes apenas para inspeções/revisões
independentes e mantenha um único escritor. Antes de editar, confirme dependências
e APIs nas versões instaladas. Ao final, rode as validações da tarefa, apresente
evidências, atualize o checklist e o Diário de execução. Pare em qualquer gate que
exija Unity Editor, conta ou Quest e forneça o roteiro exato ao usuário.
```

### 7.4 Prompt padrão de revisão de fase

```text
Revise a fase <ID> com subagentes paralelos: um para correção/arquitetura, um para
testes e um para XR/performance/conforto. Eles não devem editar arquivos. Aguarde
todos, consolide achados com caminho e linha, corrija somente problemas da fase,
execute novamente os testes e só então avalie o gate.
```

## 8. Roadmap executável

### Fase 0 — Contas, workstation e baseline

**Dependências:** nenhuma.  
**Resultado:** ambiente reproduzível, Quest conectado e repositório pronto.

- [ ] `F0-01` 👤 Concluir contas OpenAI, Unity e Meta descritas na seção 4. ⛔ — acesso ao Codex confirmado; aguardando validação humana do Unity ID/licença e da conta/organização Meta.
- [x] `F0-02` 👤 Instalar ferramentas obrigatórias da seção 5; software concluído e validado, restando cabo/headset para `F0-03`.
- [x] `F0-03` 👤/🥽 Parear o Quest, ativar Developer Mode e aceitar `Always allow from this computer` para depuração USB.
- [x] `F0-04` 👤/🤖 Validar `adb devices` e resolver conflito de múltiplos `adb` se houver.
- [ ] `F0-05` 🤖 Executar P0.1, P0.2 e P0.3.
- [x] `F0-06` 👤 Criar checkpoint Git revisado e sincronizado com GitHub, autorizado pelo titular.
- [ ] **GATE F0:** ferramentas verificadas, organização Meta válida, Quest `device/Active`, Git limpo e instruções carregadas pelo Codex.

### Fase 1 — Projeto Unity e fundação XR

**Dependências:** GATE F0.  
**Resultado:** APK vazio/arena-base abre no Quest com cabeça e duas mãos rastreadas.

#### F1.1 — Criação controlada

- [x] `F1-01` 👤 Criar o projeto **dentro deste repositório**, sem criar uma segunda pasta Git, usando `Universal 3D`/URP e a versão validada do Editor.
- [x] `F1-02` 🤖 Inspecionar o resultado antes de alterar: `ProjectVersion.txt`, `manifest.json`, `packages-lock.json`, render pipeline e build profile.
- [x] `F1-03` 🤖 Garantir estrutura `Assets/_Project/` conforme a especificação, adaptando somente se o template já trouxer convenção válida.
- [x] `F1-04` 🤖 Confirmar serialização textual, Visible Meta Files, `.gitignore` e LFS.
- [x] `F1-05` 👤/🤖 Fixar nome da empresa/produto provisórios e package ID de desenvolvimento, por exemplo `com.<organizacao>.neonfitnessvr`; documentar antes de eventual publicação.

#### F1.2 — XR e pacotes

- [x] `F1-06` 👤/🤖 Habilitar o build profile Meta Quest (ou Android se a versão não o oferecer). — Android habilitado como fallback.
- [x] `F1-07` 👤/🤖 Instalar XR Plug-in Management e Unity OpenXR Plugin; habilitar OpenXR no desktop e no Meta Quest/Android.
- [x] `F1-08` 👤/🤖 Instalar Meta XR Core SDK e Meta XR Interaction SDK pelas fontes oficiais/UPM; não instalar Platform, Voice, Avatars ou MRUK.
- [x] `F1-09` 👤/🤖 Configurar features OpenXR exigidas pelas versões instaladas e rodar `XR Plug-in Management > Project Validation`.
- [x] `F1-10` 👤/🤖 Rodar `Meta > Tools > Project Setup Tool`; aplicar correções individualmente e registrar qualquer mudança relevante.
- [x] `F1-11` 🤖 Registrar versões exatas no README; nunca escrever código contra APIs apenas lembradas de outra versão.

#### F1.3 — Rig e mãos

- [ ] `F1-12` 👤/🤖 Criar `Assets/_Project/Scenes/NeonFitnessMVP.unity`.
- [ ] `F1-13` 👤/🤖 Adicionar o rig recomendado pelo Interaction SDK instalado; remover a câmera duplicada.
- [ ] `F1-14` 👤/🤖 Definir tracking origin no nível do piso.
- [ ] `F1-15` 👤/🤖 Configurar `Hands Only`, versão padrão do hand tracking e esqueleto OpenXR.
- [ ] `F1-16` 👤/🤖 Adicionar prefabs/visualização das mãos e physics capsules/colliders recomendados pela Meta.
- [ ] `F1-17` 🤖 Criar uma cena neutra: piso, orientação de segurança, luz simples e nenhuma locomoção artificial.

#### F1.4 — Build e teste

- [ ] `F1-18` 👤/🤖 Configurar IL2CPP, ARM64 e as demais recomendações obrigatórias identificadas pelos validadores.
- [ ] `F1-19` 🤖 Compilar scripts e garantir Console sem erros novos.
- [ ] `F1-20` 👤/🥽 Gerar `Builds/Development/NeonFitnessVR-dev.apk` e executar no Quest.
- [ ] `F1-21` 👤/🥽 Confirmar câmera 6DoF, escala, piso, mão esquerda e mão direita.
- [ ] `F1-22` 👤/🥽 Repetir em standalone; Link/Simulator são apenas validações auxiliares.
- [ ] **GATE F1:** usuário confirma que o APK abre e as duas mãos são rastreadas sem controles; anexar versão, log e resultado ao diário.

### Fase 2 — Protótipo de impacto

**Dependências:** GATE F1.  
**Resultado:** um alvo fixo recebe exatamente um acerto por contato de qualquer mão.

- [ ] `F2-01` 🤖 O agente explorador identifica componentes reais do rig/mãos e pontos seguros para colliders; retorna caminhos e APIs.
- [ ] `F2-02` 🤖 Criar `Target` com estados `Active`, `Hit` e `Missed`/`Inactive`, garantindo transição atômica e acerto único.
- [ ] `F2-03` 🤖 Criar `HandHitDetector` sem buscas por frame; referências serializadas ou resolvidas uma vez.
- [ ] `F2-04` 👤/🤖 Criar prefab de alvo com collider simples, layer/tag dedicadas e material neon URP.
- [ ] `F2-05` 👤/🤖 Configurar colliders/cápsulas das mãos e matriz de colisão mínima.
- [ ] `F2-06` 🤖 Emitir evento de acerto e feedback visual leve antes de desativar/destruir.
- [ ] `F2-07` 🤖 Criar testes EditMode para garantir acerto único e ignorar estados finais.
- [ ] `F2-08` 👤/🥽 Testar alvo fixo com esquerda, direita, contato lento e gesto moderado.
- [ ] **GATE F2:** 20 tentativas alternadas sem dupla contagem e sem exigir soco forte.

### Fase 3 — Movimento, perda e geração segura

**Dependências:** GATE F2.  
**Resultado:** alvos nascem apenas na zona frontal segura, avançam e viram acerto ou perda uma única vez.

- [ ] `F3-01` 🤖 Criar `GameConfig` como `ScriptableObject` com todos os parâmetros iniciais da especificação.
- [ ] `F3-02` 🤖 Criar `TargetMover` usando `Time.deltaTime` e limite de perda relativo ao espaço do jogador.
- [ ] `F3-03` 🤖 Criar `TargetSpawner` com uma geração por vez e intervalo configurável.
- [ ] `F3-04` 🤖 Gerar posições em coordenadas do rig/jogador, dentro dos limites de altura, largura e distância.
- [ ] `F3-05` 🤖 Impedir spawn junto ao rosto, atrás do usuário, perto do chão ou acima da cabeça.
- [ ] `F3-06` 🤖 Garantir exclusividade terminal: alvo produz `Hit` ou `Miss`, nunca ambos.
- [ ] `F3-07` 🤖 Criar testes de limites de spawn, movimento independente do frame rate e transição de perda.
- [ ] `F3-08` 👤/🥽 Testar alcance confortável com pessoas/alturas disponíveis; registrar limitações da amostra.
- [ ] **GATE F3:** 30 alvos consecutivos alcançáveis sem giro, agachamento, contato com headset ou eventos duplicados.

### Fase 4 — Loop da rodada e pontuação

**Dependências:** GATE F3.  
**Resultado:** rodada completa de 60 segundos termina e reinicia de forma limpa.

- [ ] `F4-01` 🤖 Criar `GameState` (`Ready`, `Playing`, `Finished`).
- [ ] `F4-02` 🤖 Criar `GameManager` para tempo, transições, start, finish e restart.
- [ ] `F4-03` 🤖 Criar `ScoreManager` para pontos, acertos e perdas.
- [ ] `F4-04` 🤖 Conectar por eventos C#/referências serializadas, sem service locator ou dependências circulares.
- [ ] `F4-05` 🤖 Ao finalizar: bloquear spawn, impedir pontuação tardia e limpar/desativar alvos ativos.
- [ ] `F4-06` 🤖 Ao reiniciar: zerar tempo, score, contadores, corrotinas/eventos e alvos.
- [ ] `F4-07` 🤖 Testar estados, pontuação, fim, eventos tardios e reinício repetido.
- [ ] `F4-08` 👤/🥽 Executar três rodadas completas e três ciclos rápidos de reinício.
- [ ] **GATE F4:** nenhum spawn/ponto depois do fim; todos os reinícios partem de estado limpo.

### Fase 5 — UI, arena e feedback mínimo

**Dependências:** GATE F4.  
**Resultado:** MVP legível, confortável e visualmente coerente no Quest 3.

- [ ] `F5-01` 👤/🤖 Criar orientação inicial de espaço livre e movimento moderado.
- [ ] `F5-02` 🤖 Criar `GameHud` com score e tempo; opcionalmente acertos/perdas.
- [ ] `F5-03` 🤖 Criar `ResultPanel` com score, acertos, perdas e reinício.
- [ ] `F5-04` 👤/🤖 Posicionar UI em world space, frontal e fora da trajetória principal dos alvos.
- [ ] `F5-05` 👤/🤖 Criar arena escura com primitivas, grid/linhas neon e contraste suficiente.
- [ ] `F5-06` 🤖 Adicionar VFX leve; sem flashes intensos, bloom pesado ou transparências excessivas.
- [ ] `F5-07` 🤖 Não adicionar música; sons permanecem opcionais, licenciados e desligados por padrão.
- [ ] `F5-08` 👤/🥽 Avaliar leitura, fadiga, enjoo, alcance, feedback de acerto e obstrução da UI.
- [ ] **GATE F5:** usuário consegue entender, jogar, ler resultado e reiniciar sem instrução externa.

### Fase 6 — Qualidade, desempenho e build candidata

**Dependências:** GATE F5.  
**Resultado:** APK candidata atende todos os critérios do MVP com evidências.

#### Automação e revisão

- [ ] `F6-01` 🤖 Executar todos os testes EditMode e PlayMode em batch mode com a versão fixa do Unity.
- [ ] `F6-02` 🤖 Verificar Console e logs Android; nenhuma exceção/erro e nenhum aviso novo do código do projeto.
- [ ] `F6-03` 🤖 Revisar alocações por frame, buscas em `Update`, assinaturas de eventos e corrotinas abandonadas.
- [ ] `F6-04` 🤖 Revisar arquivos versionados: sem caches, builds, segredos, keystore ou assets sem licença.
- [ ] `F6-05` 🤖 Rodar revisão paralela somente leitura: correção, testes, XR/performance/conforto.
- [ ] `F6-06` 🤖 Atualizar `README.md` com versões, instalação, build, execução, parâmetros e limitações.

#### Validação no Quest

- [ ] `F6-07` 👤/🥽 Instalar APK limpa via MQDH ou `adb install -r` e abrir em `Unknown Sources`.
- [ ] `F6-08` 👤/🥽 Percorrer todo o `documentacao/testes/checklist-headset.md`.
- [ ] `F6-09` 👤/🥽 Medir frame rate/frame timing em dispositivo e investigar picos perceptíveis; registrar ferramenta e resultado.
- [ ] `F6-10` 👤/🥽 Validar perda temporária e retorno do tracking das mãos.
- [ ] `F6-11` 👤/🥽 Validar pausa/retorno do sistema e remoção/recolocação do headset.
- [ ] `F6-12` 👤/🥽 Executar ao menos cinco rodadas completas sem crash, estado residual ou piora perceptível.

#### Aceite

- [ ] `F6-13` 🤖 Mapear cada critério da seção 14 da especificação para evidência na matriz de rastreabilidade.
- [ ] `F6-14` 👤 Aprovar limitações conhecidas.
- [ ] `F6-15` 🤖 Gerar hash SHA-256 do APK candidato e registrar Unity/pacotes/commit usados.
- [ ] `F6-16` 👤 Criar tag/checkpoint Git do MVP somente após revisão.
- [ ] **GATE F6 / MVP:** todos os critérios de aceite estão marcados com evidência; APK standalone aprovada pelo usuário no Quest 3.

### Fase 7 — Publicação, somente se for autorizada depois

**Dependências:** MVP aprovado e decisão explícita de publicar. Esta fase não faz parte do MVP atual.

- [ ] `F7-01` 👤 Definir identidade final, package ID e titular da publicação.
- [ ] `F7-02` 👤 Criar app no Developer Dashboard e configurar App ID somente se necessário.
- [ ] `F7-03` 👤/🤖 Revisar requisitos atuais de loja, políticas de hand tracking, privacidade e fitness.
- [ ] `F7-04` 👤 Criar keystore de release com backup seguro; nunca versionar segredo.
- [ ] `F7-05` 👤/🤖 Gerar build release, executar validação técnica e enviar a canal de teste.
- [ ] `F7-06` 👤 Completar assets/metadados da loja, classificação, política de privacidade e Data Use Checkup aplicável.
- [ ] `F7-07` 👤 Autorizar explicitamente a submissão; o agente não publica por iniciativa própria.

## 9. Estratégia de testes e evidências

| Camada | Executado por | O que cobre | Evidência mínima |
|---|---|---|---|
| EditMode | Codex/Unity Test Runner | estados, score, spawn bounds, acerto único | XML/log de testes |
| PlayMode | Codex/Unity Test Runner | integração do loop sem hardware | XML/log + Console limpo |
| Editor + Link/Simulator | usuário + agente | iteração de rig e interações | checklist; não vale como aceite standalone |
| Quest 3 standalone | usuário | hand tracking real, UI, conforto, ciclo completo | checklist preenchido, versão e log |
| Revisão estática | subagentes somente leitura | correção, performance, escopo, segurança | achados com arquivo/linha |
| Build reproduzível | agente principal | versão, commit, APK e hash | registro no diário |

### Comandos a definir após o projeto existir

Não inventar o caminho do Editor. Depois de escolhido, documentar variáveis específicas do projeto e criar scripts versionados em `tools/` para:

- executar EditMode em batch mode;
- executar PlayMode em batch mode;
- gerar APK de desenvolvimento;
- coletar resultados em `TestResults/`/pasta ignorada;
- calcular hash da build.

Cada script deve falhar com código diferente de zero em erro de compilação/teste e não deve embutir credenciais nem caminhos pessoais.

## 10. Critérios de pronto por tarefa

Uma tarefa só pode ser marcada `[x]` quando:

- o escopo e suas dependências estão satisfeitos;
- os arquivos alterados são listados e o diff foi revisado;
- o projeto compila sem erros introduzidos pela tarefa;
- testes relevantes passam;
- validações manuais estão registradas — quando aplicáveis;
- a documentação e este checklist refletem o estado real;
- não há segredos, caches, builds ou assets sem licença no commit;
- existe próximo passo claro ou o gate foi formalmente aprovado.

“Código escrito”, “funciona no Editor” ou “o agente acredita que funciona” não são evidências suficientes para um gate que exige o Quest.

## 11. Bloqueios e decisões pendentes

Registrar aqui apenas itens ativos. Remover da lista quando resolvidos e preservar a decisão no diário/ADR.

- [ ] Confirmar se MQDH já existe em caminho não padrão antes de instalar; Unity Hub/Editor já foram localizados e validados.
- [ ] `F0-01`: validar Unity ID/organização/licença no Unity Hub e conta/organização Meta no Developer Dashboard; essas etapas exigem login, aceite de termos e eventual verificação de identidade pelo titular.
- [ ] Confirmar elegibilidade/licença Unity escolhida pelo titular da conta.
- [ ] Confirmar criação/verificação da organização Meta e Developer Mode.
- [ ] Confirmar o patch exato do Unity 6.3 LTS recomendado pelo MQDH no momento de `F1-01`.
- [ ] Definir `<organizacao>` para o package ID de desenvolvimento.
- [ ] Definir se haverá repositório remoto e quem terá acesso.

## 12. Diário de execução

### 2026-09-19 — levantamento e criação do plano — agente principal

- Resultado: concluído.
- Arquivos observados: `projeto_neon_fitness_vr_mvp.md`.
- Arquivo criado: `documentacao/planos/plano-implementacao-codex-vscode.md`.
- Ferramentas confirmadas: Git 2.49.0, Git LFS 3.6.1, VS Code 1.138.0, extensão Codex e CLI embarcado.
- Não detectados: Unity/Unity Hub e ADB no `PATH`; extensão Unity da Microsoft na lista de extensões.
- Estado do projeto: somente especificação; nenhum projeto Unity ou commit ainda.
- Próximo passo: `F0-01` a `F0-06`.

### 2026-09-19 — F0-01 — Codex

- Resultado: parcial; bloqueado nas validações pessoais de Unity e Meta.
- Arquivos alterados: `documentacao/planos/plano-implementacao-codex-vscode.md`.
- Validação: `code --list-extensions --show-versions` confirmou `openai.chatgpt@26.908.40401`; a sessão do Codex leu e resumiu `documentacao/projeto_neon_fitness_vr_mvp.md` no workspace.
- Evidência: extensão instalada, sessão autenticada e acesso de leitura ao workspace confirmados; nenhuma credencial foi lida ou registrada.
- Decisões: nenhuma.
- Próximo passo: o titular deve confirmar Unity ID/organização/licença ativa e conta/organização Meta verificada; depois, concluir `F0-01` e seguir para `F0-02`.

### 2026-09-19 — F0-02 — Codex

- Resultado: concluído para todo o software obrigatório; cabo e headset serão validados em `F0-03`.
- Arquivos alterados: `documentacao/planos/plano-implementacao-codex-vscode.md`.
- Instalação: Unity Hub `3.21.3`; Unity CLI `1.0.0-beta.8`; Editor Unity `6000.3.24f1`; Android Build Support; SDK/NDK Tools; OpenJDK; extensão Unity do VS Code `1.3.1`; MQDH `6.4.1`; driver Oculus ADB `12.0.0.0`.
- Validação: `unity editors verify 6000.3.24f1` retornou `ok` para o Editor e todos os módulos; `adb version` retornou `1.0.41`/platform-tools `36.0.0`; Java retornou Temurin `17.0.18+8`; NDK retornou `27.2.12479018`; `code --list-extensions --show-versions` confirmou Unity/C#/C# Dev Kit; registro de aplicativos e assinatura Authenticode confirmaram MQDH `6.4.1` assinado pela Meta; `pnputil /enum-drivers` confirmou `android_winusb.inf` assinado pela Oculus.
- Evidência: inventário do Unity CLI lista `6000.3.24f1` com `Android Build Support`, `Android SDK & NDK Tools` e `OpenJDK`; licença Unity Personal ativa. O usuário aceitou explicitamente os termos do Android SDK antes da instalação. MQDH e driver foram baixados de `securecdn.oculus.com` a partir dos metadados oficiais da Meta e suas assinaturas digitais foram verificadas antes da instalação.
- Observação: também existe Unity `6000.6.2f1` com Documentation e Web Build Support; não será usado neste projeto e foi preservado.
- Próximo passo: concluir `F0-01` confirmando a situação da conta/organização Meta e executar `F0-03` conectando o Quest com cabo de dados, Developer Mode e autorização USB.

### 2026-09-19 — P0.1–P0.3 e F1-01–F1-05 — Codex

- Resultado: projeto-base iniciado; tarefas locais de preparação concluídas, exceto o checkpoint Git e a confirmação do `AGENTS.md` em uma nova conversa.
- Arquivos criados: projeto Unity (`Assets/`, `Packages/`, `ProjectSettings/`), `.gitignore`, `.gitattributes`, `AGENTS.md`, `README.md`, ADRs e documentação de testes.
- Stack resolvida: Unity `6000.3.24f1`, template `3D URP`, URP `17.3.0`, Input System `1.20.0` e Unity Test Framework `1.6.0`.
- Configuração: `Force Text`, `Visible Meta Files`, namespace `NeonFitnessVR`, empresa/produto `Neon Fitness VR` e package ID Android provisório `com.neonfitnessvr.prototype`.
- Validação: abertura batch encerrou com código `0`, compilação de scripts retornou `ExitCode: 0` e o Unity CLI reconheceu o projeto, a versão do Editor, Android/IL2CPP e os pacotes resolvidos.
- Decisão: por solicitação do usuário, o projeto local avançou apesar de `F0-03`/`F0-04` ainda dependerem do headset; nenhuma etapa XR ou gate de hardware foi marcada como concluída.
- Próximo passo: revisar e criar `F0-06` (checkpoint Git); depois conectar o Quest para `F0-03`/`F0-04` antes de configurar XR e gerar APK.

### 2026-09-19 — F0-03 e F0-04 — Codex

- Resultado: concluído.
- Validação: ADB identificou o headset `Quest 3` como `device`, com Android `14`; Developer Mode e autorização USB estão efetivos.
- Observação: há um segundo dispositivo em estado `unauthorized` e um emulador Android conectados. Comandos destinados ao Quest devem usar explicitamente o serial do headset para evitar o alvo incorreto.
- Próximo passo: criar o checkpoint Git `F0-06` antes de configurar os pacotes XR (`F1-06` em diante).

### 2026-09-19 — F0-06 — Codex

- Resultado: checkpoint Git autorizado pelo titular; fluxo de branches seguirá Gitflow com `master`, `develop` e `feature/*`.
- Privacidade: a autoria local será configurada com e-mail `noreply` do GitHub antes do primeiro push; commits automatizados incluem o trailer `Assisted-by: Codex (OpenAI)`.
- Próximo passo: sincronizar a feature de bootstrap, integrá-la em `develop` e iniciar `F1-06` somente após revisão de compatibilidade dos pacotes XR.

### 2026-09-20 — F1-06 e F1-07 — Codex

- Resultado: concluído.
- Configuração: build target Android ativo; XR Plug-in Management `4.5.3` e Unity OpenXR Plugin `1.16.1`; loader OpenXR atribuído para Standalone e Android.
- Validação: execução batch do Unity encerrou com código `0`, gerou os assets XR, atualizou o lockfile e compilou scripts sem erros.
- Decisão: Meta XR Core SDK e Meta XR Interaction SDK continuam pendentes porque sua distribuição oficial via Asset Store exige que o titular adicione os pacotes à conta Unity; não foram instalados pacotes Platform, Voice, Avatars ou MRUK.
- Próximo passo: adicionar Meta XR Core SDK e Meta XR Interaction SDK na página “My Assets” da conta Unity para concluir `F1-08`.

### 2026-09-20 — F1-08 a F1-11 — Codex

- Resultado: concluído.
- Pacotes: Meta XR Core SDK `205.0.0` e Meta XR Interaction SDK `205.0.0`, obtidos pela Asset Store; `Meta XR Interaction SDK Essentials` foi instalado como dependência. Não há pacotes Meta Platform, Voice, Avatars ou MRUK no manifesto.
- OpenXR Android: habilitados Meta Quest Support, Foveated Rendering e Subsampled Layout. A validação OpenXR não retornou erros bloqueantes; os avisos de perfis de controle/mãos e otimizações opcionais foram adiados para as etapas específicas de interação e desempenho.
- Project Setup Tool: aplicadas individualmente as correções para Android API mínima `32`, target API `34`, entrada única `GameActivity` e Subsampled Layout. Configuração de DUC/App ID foi deixada fora de escopo porque o projeto não utiliza APIs da Meta Platform.
- Validação: o Unity em batch encerrou com código `0` após aplicar e conferir as configurações; as versões exatas foram registradas no README.
- Próximo passo: criar a cena `NeonFitnessMVP` (`F1-12`).

## 13. Fontes oficiais consultadas

Revalidar versões e requisitos no início da Fase 0 e antes de publicação, pois SDKs, políticas e ferramentas mudam.

- OpenAI: [Extensão do Codex para IDE](https://developers.openai.com/pt-BR/docs/codex/ide), [subagentes no Codex](https://developers.openai.com/pt-BR/docs/agent-configuration/subagents) e [instruções com AGENTS.md](https://developers.openai.com/docs/agent-configuration/agents-md).
- Meta: [Software Setup](https://developers.meta.com/horizon/design/prototype-setup-software/), [configuração do headset](https://developers.meta.com/horizon/documentation/unity/unity-env-device-setup/), [configuração do projeto Unity](https://developers.meta.com/horizon/documentation/unity/unity-project-setup/) e [XR Plugin Management](https://developers.meta.com/horizon/documentation/unity/unity-xr-plugin/).
- Meta: [Hands Setup](https://developers.meta.com/horizon/documentation/unity/unity-handtracking-hands-setup/), [visão geral de hand tracking](https://developers.meta.com/horizon/documentation/unity/unity-handtracking-overview/) e [pacotes/requisitos do Interaction SDK](https://developers.meta.com/horizon/documentation/unity/unity-isdk-packages-and-requirements/).
- Meta: [visão dos SDKs](https://developers.meta.com/horizon/documentation/unity/unity-sdks-overview/), [Project Setup Tool](https://developers.meta.com/horizon/documentation/unity/unity-upst-overview/) e [verificação de organização](https://developers.meta.com/horizon/resources/publish-organization-verification/).
- Unity: [Unity 6 e versões LTS](https://unity.com/releases/unity-6), [configuração do ambiente Android](https://docs.unity3d.com/6000.1/Documentation/Manual/android-sdksetup.html) e [Unity Personal](https://unity.com/products/unity-personal).
- Microsoft: [desenvolvimento Unity no VS Code](https://code.visualstudio.com/docs/other/unity).
