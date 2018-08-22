<template>
    <div ref="cardRef" class="card-tool float-right">
        <em v-if="refresh" @click="handleRefresh" class="fas fa-sync"></em>
        <em v-if="dismiss" @click="handleDismiss" class="fa fa-times"></em>
    </div>
</template>
<script>
    // Card Tools
    // -----------------------------------
    import $ from 'jquery';
    import 'animo/animo';

    const checkRequiredProps = (props, propName, componentName) => {
      if (!props.dismiss && !props.refresh) {
        return new Error(`One of 'dismiss' or 'refresh' is required by '${componentName}' component.`)
      }
    }

    /**
     * Add action icons to card components to allow
     * refresh data or remove a card element
     */
    export default {
        name: 'CardTool',
        props: {
            /** show the refreshe icon */
            refresh: Boolean,
            /** show the remove icon */
            dismiss: Boolean,
            /** triggers before card is removed */
            onRemove: {
                type: Function,
                default: () => {}
            },
            /** triggers after card was removed */
            onRemoved: {
                type: Function,
                default: () => {}
            },
            /** triggers when user click on refresh button */
            onRefresh: {
                type: Function,
                default: () => {}
            },
            /** name if the icon class to use as spinner */
            spinner: {
                type: String,
                default: 'standard'
            }
        },

        methods: {
            handleDismiss (e) {
                // find the first parent card
                const card = $(this.$refs.cardRef).closest('.card');

                const removeCol = cols => {
                    cols
                        .filter((idx, el) => ($(el).is('[class*="col-"]:not(.sortable)') && !el.firstChild))
                        .remove();
                }

                const destroyCard = () => {
                    const col = card.parent();
                    // remove card
                    card.remove();
                    // remove parent column if is empty
                    removeCol(col);
                    // An event to catch when the card has been removed from DOM
                    this.onRemoved();
                }

                const confirmRemove = () => card.animo({ animation: 'bounceOut' }, destroyCard)

                // Trigger the event and finally remove the element
                this.onRemove(card, confirmRemove);

            },
            handleRefresh (e) {
                const whirlClass = 'whirl';
                const card = $(this.$refs.cardRef).parents('.card').eq(0)

                // method to clear the spinner when done
                const done = () => { card.removeClass(whirlClass); }
                // start showing the spinner
                card.addClass(whirlClass + ' ' + this.spinner);

                // event to remove spinner when refres is done
                this.onRefresh(card, done);
            }
        }


    }


</script>