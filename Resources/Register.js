const apiCadastroUrl = "https://localhost:44339/api/Usuarios/InserirUsuario"; // URL da API de cadastro

document.getElementById("registerForm").addEventListener("submit", async (event) => {
    event.preventDefault();

    const nome = document.getElementById("nome").value;
    const email = document.getElementById("email").value;
    const senha = document.getElementById("senha").value;
    const errorMessage = document.getElementById("error-message");
    const loadingSpinner = document.getElementById("loadingSpinner");

    loadingSpinner.style.display = "block";
    
    setTimeout(async () => {
        try {
            // Fazendo a requisição POST para o backend
            const response = await fetch(apiCadastroUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ nome, email, senha })
            });

            if (!response.ok) {
                // Obtendo detalhes do erro do backend (se disponível)
                const errorData = await response.json();
                console.error("Erro no servidor:", errorData);
                throw new Error(errorData.message || "Erro ao cadastrar usuário");
            }

            const data = await response.json();
            console.log("Usuário cadastrado com sucesso:", data);

            window.location.href = "login.html";

        } catch (error) {
            // Verifica se é um erro com mensagem ou outro tipo
            const mensagemErro = error instanceof Error ? error.message : "Erro desconhecido.";
            console.error("Erro ao tentar cadastrar:", mensagemErro);

            // Exibindo a mensagem de erro para o usuário
            errorMessage.textContent = mensagemErro;

        } finally {
            loadingSpinner.style.display = "none";
        }
    }, 500);
});
