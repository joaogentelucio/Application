const togglePassword = document.getElementById('togglePassword');
        const senhaInput = document.getElementById('senha');

        togglePassword.addEventListener('click', () => {
            // Alterna o tipo do input entre "password" e "text"
            const type = senhaInput.getAttribute('type') === 'password' ? 'text' : 'password';
            senhaInput.setAttribute('type', type);

            // Alterna o ícone do olho
            togglePassword.classList.toggle('fa-eye');
            togglePassword.classList.toggle('fa-eye-slash');
        });
        (function() {
            'use strict';
            window.addEventListener('load', function() {
              // Pega todos os formulários que nós queremos aplicar estilos de validação Bootstrap personalizados.
              var forms = document.getElementsByClassName('needs-validation');
              // Faz um loop neles e evita o envio
              var validation = Array.prototype.filter.call(forms, function(form) {
                form.addEventListener('submit', function(event) {
                  if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                  }
                  form.classList.add('was-validated');
                }, false);
              });
            }, false);
          })();