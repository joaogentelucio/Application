const apiUrl = "https://localhost:44339/api/Auth/login"; // URL da API de autenticação

document.getElementById("loginForm").addEventListener("submit", async (event) => {
    event.preventDefault();

    const email = document.getElementById("email").value;
    const senha = document.getElementById("senha").value;
    const errorMessage = document.getElementById("error-message");
    const loadingSpinner = document.getElementById("loadingSpinner");

    loadingSpinner.style.display = "block";

    setTimeout(async () => {

        try {
            // Requisição POST para o backend
            const response = await fetch(apiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ email, senha })
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.error("Erro no servidor", errorData);
                throw new Error(errorData.message || "Erro ao fazer login");
                //throw new Error("Credenciais inválidas. Tente novamente.");
            }

            const data = await response.json();

            // Verifica se o email e senha já estão salvos
            const emailSalvo = localStorage.getItem("emailUsuario");
            const senhaSalva = localStorage.getItem("senhaUsuario");

            if (!emailSalvo && !senhaSalva) {
                // Salva email e senha no localStorage caso ainda não estejam armazenados
                localStorage.setItem("emailUsuario", email);
                localStorage.setItem("senhaUsuario", senha);
                console.log("Email e senha salvos no navegador.");
            } else {
                console.log("Email e senha já estavam salvos.");
            }

            // Armazena o token JWT no localStorage
            localStorage.setItem("token", data.token);

            // Redireciona para a Dashboard
            window.location.href = "Pages/Dashboard.html";

        } catch (error) {

            console.error("Erro:", error);
            errorMessage.textContent = error.message;
        }
        finally {
            loadingSpinner.style.display = "none";
        };
    }, 500);
});