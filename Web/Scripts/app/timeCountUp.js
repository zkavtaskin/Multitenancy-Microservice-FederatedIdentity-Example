
var TimeCountUp = {
    selector: null,
    dateTimeSelector: null,
    valueSelector: null,
    updateInterval: 1000,

    init: function (settings) {
        this.selector = settings.selector;
        this.dateTimeSelector = settings.dateTimeSelector;
        this.valueSelector = settings.valueSelector;
        this.updateInterval = settings.updateInterval;
    },

    start: function () {
        setInterval(TimeCountUp._updateTimers, TimeCountUp.updateInterval);
    },

    _updateTimers: function () {
        $(TimeCountUp.selector).each(function (index, element) {
            var dateTimeVal = $(element).find(TimeCountUp.dateTimeSelector).val();

            var dateTime = new Date(parseInt(dateTimeVal));

            var timeSpan = TimeCountUp._getTimeSpan(new Date() - dateTime);

            var strFormated = TimeCountUp._pad(timeSpan.days, 2) +
                                '.' + TimeCountUp._pad(timeSpan.hours, 2) +
                                ':' + TimeCountUp._pad(timeSpan.mins, 2) +
                                ':' + TimeCountUp._pad(timeSpan.seconds, 2);

            $(element).find(TimeCountUp.valueSelector).html(strFormated);
        });
    },

    _getTimeSpan: function (length) {
        var timeSpan = {};

        timeSpan.days = Math.floor(length / (1000 * 60 * 60 * 24));
        length -= timeSpan.days * (1000 * 60 * 60 * 24);

        timeSpan.hours = Math.floor(length / (1000 * 60 * 60));
        length -= timeSpan.hours * (1000 * 60 * 60);

        timeSpan.mins = Math.floor(length / (1000 * 60));
        length -= timeSpan.mins * (1000 * 60);

        timeSpan.seconds = Math.floor(length / (1000));
        length -= timeSpan.seconds * (1000);

        return timeSpan;
    },

    _pad: function (str, max) {
        str = str.toString();
        return str.length < max ? TimeCountUp._pad("0" + str, max) : str;
    }
}