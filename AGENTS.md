# Neon Fitness VR — instruções para agentes

## Escopo e fontes

- Leia `documentacao/projeto_neon_fitness_vr_mvp.md` e `documentacao/planos/plano-implementacao-codex-vscode.md` antes de implementar tarefas.
- Execute somente a tarefa selecionada e não antecipe funcionalidades fora do MVP.
- Stack fixa inicial: Unity `6000.3.24f1`, Universal Render Pipeline `17.3.0`; versões XR devem ser fixadas e documentadas somente após inspeção no Package Manager.
- Arquitetura de código: responsabilidades pequenas em `NeonFitnessVR.<Area>`, com dependências explícitas; código próprio fica em `Assets/_Project/`.

## Arquivos Unity

- Nunca edite conteúdo gerado em `Library/`, `Temp/`, `Logs/`, `obj/`, builds ou `PackageCache`.
- Preserve todo arquivo `.meta`; nunca mova ou renomeie um asset sem mover ou renomear seu `.meta` junto.
- Mantenha `Asset Serialization = Force Text` e `Version Control = Visible Meta Files`.
- Cenas, prefabs e configurações Unity têm um único agente escritor por vez.
- Não avance gates que dependam de headset, Developer Mode, USB ou teste de conforto sem confirmação humana e evidência real.

## Validação

- Integridade: `unity projects verify C:\projetos\neon-fitness-vr`
- Informações do projeto: `unity projects info C:\projetos\neon-fitness-vr`
- Testes EditMode/PlayMode: usar o Unity Test Framework em modo batch quando houver testes.
- Android/Quest: usar o `adb` embarcado no Editor e exigir estado `device` antes de instalar APK.
- Antes de concluir, verifique erros do Editor, testes relevantes, `git diff --check` e `git status --short`.

## Code Review Rules

- Trate compatibilidade de versões, permissões Android, OpenXR/Meta XR e perda de rastreamento como riscos explícitos.
- Preserve conforto: evite movimento involuntário de câmera, flashes agressivos, alvos fora do alcance e posturas desconfortáveis.
- Proteja desempenho standalone: evite alocações por frame, física desnecessária, materiais/efeitos caros e excesso de draw calls.
- Revise colisões para impedir dupla pontuação e eventos repetidos entre mãos, colliders e frames consecutivos.
- Exija teste no headset para rastreamento, alcance, legibilidade, conforto e desempenho; teste no Editor não substitui essa evidência.
