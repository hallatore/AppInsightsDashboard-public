/// <binding ProjectOpened='watch:buildJs, watch:buildCss' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        clean: {
            build: ['bundles/*']
        },

        babel: {
            options: {
                sourceMap: true,
                presets: ['react', 'babel-preset-es2015']
            },
            dist: {
                files: [{
                    expand: true,
                    cwd: 'Scripts/Components',
                    src: ['*.jsx'],
                    dest: 'bundles/jsx/',
                    ext: '.js'
                }]
            }
        },

        less: {
            development: {
                options: {
                    paths: ['Content/El/'],
                    yuicompress: false
                },
                files: [{
                    expand: true,
                    cwd: 'Content/El/',
                    src: ['*.less'],
                    dest: 'bundles/less/',
                    ext: '.css'
                },
                { "bundles/Reset.css": "Content/Reset.less" }]
            }
        },

        concat: {
            options: {
                separator: '\n'
            },
            js: {
                src: [
                    'Scripts/Libs/bind.js',
                    'Scripts/Libs/jquery-3.0.0.min.js',
                    'Scripts/Libs/react.min.js',
                    'Scripts/Libs/react-dom.min.js',
                    'bundles/jsx/*.js',
                    'Scripts/Libs/init.js'],
                dest: 'bundling/_main.js'
            },
            css: {
                src: ['bundles/less/*.css'],
                dest: 'bundles/less.css'
            }
        },

        uglify: {
            options: {
                sourceMap: true
            },
            build: {
                files: {
                    'bundling/main.js': ['bundling/_main.js']
                }
            }
        },

        cssmin: {
            options: {
                rebase: true,
                relativeTo: 'Content/',
                target: 'Content/',
                keepSpecialComments: 0, 
                advanced: true
            },
            bin: {
                files: [{
                    src: [
                       'bundles/Reset.css',
                       'Content/Roboto/roboto.css',
                       'bundles/less.css'
                    ],
                    dest: 'bundling/main.css'
                }]
            }
        },

        watch: {
            buildCss: {
                files: ['Content/**/*.less', 'Content/**/*.css'],
                tasks: ['buildCss']
            },
            buildJs: {
                files: ['Scripts/**/*.jsx', 'Scripts/**/*.js'],
                tasks: ['buildJs']
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-babel');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('buildCss', ['clean', 'less', 'concat', 'cssmin', 'clean']);
    grunt.registerTask('buildJs', ['clean', 'babel', 'concat', 'uglify', 'clean']);
    grunt.registerTask('build', ['clean', 'babel', 'less', 'concat', 'uglify', 'cssmin', 'clean']);
};