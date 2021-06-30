
let stripe, customer, price, card;
stripe = window.Stripe('pk_test_51HsZbzBkXnJ98OJr1J1FhYXipd6r7zkVfiCSxqyAKyvtVQukCfYD8FIDt5cRqJ5HT7aSclkwIOLabGhY7OXmqbfX00KesyaFAy');
let elements = stripe.elements();
let  style = {
    base: {
        color: '#32325d',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
            color: '#aab7c4'
        }
    },
    invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
    }
};
let startcard = true;
function Initiate() {
    if (startcard) {
        card = elements.create('card', { style: style });
        startcard = false;
    }
    card.mount('#card-element');
    card.on('change', function (event) {
        displayError(event);
    });
}

function displayError(event) {

    let displayError = document.getElementById('card-element-errors');

    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
}

function createPaymentMethod(dotnetHelper, cardElement, billingemail, billingName)
{
    return stripe
        .createPaymentMethod({
            type: 'card',
            card: cardElement,
            billing_details: {
                name: billingName,
                email: billingemail,
            },
        })
        .then((result) => {
            if (result.error) { 
                
                displayError(result);


            } else {

                createSubscription(dotnetHelper, result.paymentMethod.id );
            }
        });
}

function createPaymentMethodServer(dotnetHelper, billingemail, billingName)
{
    createPaymentMethod(dotnetHelper, card, billingemail, billingName);
}

function createSubscription(dotnetHelper, paymentMethodId)
{
    dotnetHelper.invokeMethodAsync('Subscribe', paymentMethodId);
    dotnetHelper.dispose();
}


