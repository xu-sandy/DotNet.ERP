/*
 * jQuery UI Multilevel Accordion v.1
 * 
 * Copyright (c) 2011 Pieter Pareit
 *
 * http://www.scriptbreaker.com
 *
 */

//plugin definition
(function ($) {
    $.fn.extend({

        //pass the options variable to the function
        accordion: function (options) {

            var defaults = {
                accordion: 'true',
                speed: 300,
                closedSign: 'closedSign',
                openedSign: 'openedSign'
            };

            // Extend our default options with those provided.
            var opts = $.extend(defaults, options);
            //Assign current element to variable, in this case is UL element
            var $this = $(this);

            //add a mark [+] to a multilevel menu
            var topIndex = 1;
            $this.find("li").each(function () {
                var $afirst = $(this).find("a:first");
                $afirst.attributes = (new Function("return " + $.trim($afirst.attr("data-options"))))();
                if ($afirst.attributes["level"] == 0) {
                    $afirst.append("<span class='color color" + (topIndex % 4+1) + "'></span>");
                    topIndex++;
                }
                if ($(this).find("ul").size() != 0) {
                    //add the multilevel sign next to the link
                    $afirst.children().first().before("<span class='" + opts.closedSign + "'></span>");

                    //avoid jumping to the top of the page when the href is an #
                    if ($afirst.attr('href') == "#") {
                        $afirst.click(function () { return false; });
                    }
                }
                else {
                    if ($afirst.attributes["level"] == "0") {
                        $afirst.css("padding-left", "20px");
                    }
                }
                $afirst.click(function () {
                    if ('url' in $afirst.attributes) {
                        if ($afirst.attributes.url!="")
                            jump($afirst.attributes.text, $afirst.attributes.url, $afirst.attributes.id);
                    }
                });
            });

            //open active level
            $this.find("li.active").each(function () {
                $(this).parents("ul").slideDown(opts.speed);
                $(this).parents("ul").parent("li").find("span:first").removeClass(opts.closedSign).addClass(opts.openedSign);
            });

            $this.find("li a").click(function () {
                if ($(this).parent().find("ul").size() != 0) {
                    if (opts.accordion) {
                        //Do nothing when the list is open
                        if (!$(this).parent().find("ul").is(':visible')) {
                            parents = $(this).parent().parents("ul");
                            visible = $this.find("ul:visible");
                            visible.each(function (visibleIndex) {
                                var close = true;
                                parents.each(function (parentIndex) {
                                    if (parents[parentIndex] == visible[visibleIndex]) {
                                        close = false;
                                        return false;
                                    }
                                });
                                if (close) {
                                    if ($(this).parent().find("ul") != visible[visibleIndex]) {
                                        $(visible[visibleIndex]).slideUp(opts.speed, function () {
                                            $(this).parent("li").find("span:first").removeClass(opts.openedSign).addClass(opts.closedSign);
                                        });

                                    }
                                }
                            });
                        }
                    }
                    if ($(this).parent().find("ul:first").is(":visible")) {
                        $(this).parent().find("ul:first").slideUp(opts.speed, function () {
                            $(this).parent("li").find("span:first").delay(opts.speed).removeClass(opts.openedSign).addClass(opts.closedSign);
                        });


                    } else {
                        $(this).parent().find("ul:first").slideDown(opts.speed, function () {
                            $(this).parent("li").find("span:first").delay(opts.speed).removeClass(opts.closedSign).addClass(opts.openedSign);
                        });
                    }
                }
            });
        }
    });
})(jQuery);