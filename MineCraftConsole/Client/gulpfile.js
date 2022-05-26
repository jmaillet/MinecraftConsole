'use strict';

import gulp from 'gulp';
import compiler from 'sass'
import gulpSass from 'gulp-sass';

const sass = gulpSass(compiler)

export function build() {
    return gulp.src('./Scss/**/app.scss')
        .pipe(sass.sync({ outputStyle: 'compressed' }).on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/css'));
}

export function watch() {
    gulp.watch("./Scss/**/*.scss", done => {
        build();
        done();
    });
}
